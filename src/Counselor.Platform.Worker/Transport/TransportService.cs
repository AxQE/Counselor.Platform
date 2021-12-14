using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Services;
using Counselor.Platform.Utils;
using Counselor.Platform.Worker.Transport.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Transport
{
	//todo: большая часть логики относится больше к ядру, чем к транспортной части, всё за исключением фабричного метода  можно перенести в Counselor.Platform.
	public class TransportService : BackgroundService, IDisposable
	{
		private readonly ILogger<TransportService> _logger;
		private readonly IServiceProvider _serviceProvider;
		private readonly ServiceOptions _options;

		private readonly HashSet<TransportData> _runningBots = new HashSet<TransportData>();

		public TransportService(
			ILogger<TransportService> logger,
			IServiceProvider serviceProvider,
			IOptions<ServiceOptions> options
			)
		{
			_logger = logger;
			_serviceProvider = serviceProvider;
			_options = options.Value;
		}

		protected override async Task ExecuteAsync(CancellationToken token)
		{
			await InitializeTransportsAsync();

			while (!token.IsCancellationRequested)
			{
				try
				{
					using (var database = _serviceProvider.GetRequiredService<IPlatformDatabase>())
					{
						await StartBotsAsync(database);
						await StopBotsAsync(database);
					}
				}
				catch (Exception ex)
				{
					_logger.LogCritical(ex, "Transport Service main loop error.");
				}

				await Task.Delay(_options.ServiceIntervalMs, token);
			}
		}

		//todo: этого тут не должно быть, нужо перенести в Counselor.Platform
		private async Task InitializeTransportsAsync()
		{
			using (var database = _serviceProvider.GetRequiredService<IPlatformDatabase>())
			{
				var dbTransports = new List<Data.Entities.Transport>();

				foreach (var transport in _options.Transports)
				{
					if (string.IsNullOrEmpty(transport.SystemName)) continue;

					var dbTransport = await database
						.Transports
						.FirstOrDefaultAsync(x => x.Name.Equals(transport.SystemName)); //todo: нужно приводить в один регистр имя системы

					if (dbTransport == null)
					{
						await database.Transports.AddAsync(
							new Data.Entities.Transport
							{
								Name = transport.SystemName,
								IsActive = transport.IsEnabled
							});

						await database.SaveChangesAsync();
					}
					else
					{
						if (dbTransport.IsActive != transport.IsEnabled)
						{
							dbTransport.IsActive = transport.IsEnabled;
							database.Transports.Update(dbTransport);
							await database.SaveChangesAsync();
						}
					}

					dbTransports.Add(dbTransport);
				}

				var dbCommands = await database.Commands
					.Include(x => x.Transport)
					.ToListAsync();

				foreach (var type in TypeHelpers.GetTypesWithAttribute<InterpreterCommandAttribute>())
				{
					var transport = dbTransports.FirstOrDefault(x => type.FullName.Contains(x?.Name));

					if (!dbCommands.Exists(x => x.Name.Equals(type.Name) && transport?.Id == x.Transport?.Id))
					{
						var (isActive, parameters) = MergeAttributes(TypeHelpers.GetAppliedAttributes<InterpreterCommandAttribute>(type));

						await database.Commands.AddAsync(
							new Command
							{
								Name = type.Name,
								IsActive = isActive,
								Paramaters = parameters
									.Select(x =>
										new CommandParameter
										{
											Name = x.Item1,
											Type = x.Item2
										}).ToList(),
								Transport = transport
							});
					}
				}

				await database.SaveChangesAsync();
			}
		}

		private static (bool isActive, IEnumerable<(string, string)> parameters) MergeAttributes(IEnumerable<InterpreterCommandAttribute> attributes)
		{
			bool isActive = true;
			var parameters = new List<(string, string)>();

			foreach (var attribute in attributes)
			{
				if (!attribute.IsActive) isActive = false;
				parameters.Add((attribute.ParameterName, attribute.ParameterType.Name));
			}

			return (isActive, parameters);
		}

		public async Task StartBotsAsync(IPlatformDatabase database)
		{
			var bots = await database.Bots
				.Where(x => (x.BotState == BotState.Pending || x.BotState == BotState.Started)
					&& !_runningBots.Select(e => e.Bot.Id).Contains(x.Id))
				.Include(x => x.Transport)
				.Include(x => x.Owner)
				.Include(x => x.Script)
				.ToListAsync();

			if (bots.Any())
			{
				foreach (var bot in bots)
				{
					try
					{
						EnsureValidity(bot);

						var transportData = CreateTransport(bot);
						_runningBots.Add(transportData);
						await transportData.Transport.StartAsync(transportData.CancellationToken);
						bot.BotState = BotState.Started;
					}
					catch (Exception ex)
					{
						bot.BotState = BotState.Invalid;
						_logger.LogError(ex, $"Error during bot creation. BotId: {bot.Id}.");
					}

					database.Bots.Update(bot);
				}

				await database.SaveChangesAsync();
			}
		}

		public async Task StopBotsAsync(IPlatformDatabase database)
		{
			var lastExecution = DateTime.Now.AddMilliseconds(-1 * _options.ServiceIntervalMs);

			var stoppedBots = await database.Bots
				.AsNoTracking()
				.Where(x => x.BotState == BotState.Stopped && x.ModifiedOn > lastExecution)
				.ToListAsync();

			var transportsData = _runningBots.Where(x => stoppedBots.Contains(x.Bot));

			if (transportsData.Any())
			{
				foreach (var transportData in transportsData)
				{
					transportData.Bot.BotState = BotState.Stopped;
					await transportData.Transport.StopAsync();
					_runningBots.Remove(transportData);
				}
			}
		}

		private static void EnsureValidity(Bot bot)
		{
			string invalidParameter = string.Empty;

			if (string.IsNullOrEmpty(bot.Script?.Instruction))
			{
				invalidParameter = nameof(bot.Script.Instruction);
			}
			else if (string.IsNullOrEmpty(bot.Transport?.Name))
			{
				invalidParameter = nameof(bot.Transport.Name);
			}
			else if (bot.Owner == null)
			{
				invalidParameter = nameof(bot.Owner);
			}
			else if (string.IsNullOrEmpty(bot.Configuration))
			{
				invalidParameter = nameof(bot.Configuration);
			}

			if (!string.IsNullOrEmpty(invalidParameter))
				throw new ArgumentException($"Bot is not valid. Invalid paramater: {invalidParameter}. BotId: {bot.Id}.");
		}

		private TransportData CreateTransport(Bot bot) => bot.Transport.Name switch
		{
			"Telegram" => new TransportData(bot,
				new TelegramService(
										   _serviceProvider.GetRequiredService<ILogger<TelegramService>>(),
										   Options.Create(JsonSerializer.Deserialize<TelegramOptions>(bot.Configuration)),
										   _serviceProvider.GetRequiredService<IBehaviorExecutor>(),
										   _serviceProvider.GetRequiredService<IPlatformDatabase>(),
										   new ServiceContext
										   {
											   BotId = bot.Id,
											   OwnerId = bot.Owner.Id,
											   ScriptId = bot.Script.Id,
											   TransportId = bot.Transport.Id,
											   TransportName = bot.Transport.Name
										   }
										   ),
				CancellationToken.None
				),
			"Discord" => throw new NotImplementedException("Discord service is not implemented."),
			_ => throw new ArgumentOutOfRangeException(nameof(bot.Transport.Name), $"Unrecognized transport name. Transport name: {bot.Transport.Name}. BotId: {bot.Id}.")
		};
	}

	class TransportData
	{
		public Bot Bot { get; }
		public IngoingServiceBase Transport { get; }
		public CancellationToken CancellationToken { get; }

		public TransportData(Bot bot, IngoingServiceBase transport, CancellationToken token)
		{
			Bot = bot ?? throw new ArgumentNullException(nameof(bot));
			Transport = transport ?? throw new ArgumentNullException(nameof(transport));
			CancellationToken = token;
		}

		public override int GetHashCode()
		{
			return Bot.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj is not TransportData data)
				return false;

			return Bot.Id == data.Bot.Id;
		}
	}
}
