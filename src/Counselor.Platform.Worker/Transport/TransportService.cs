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
		private readonly IPlatformDatabase _database;
		private readonly IServiceProvider _serviceProvider;
		private readonly TransportServiceOptions _options;

		private readonly HashSet<TransportData> _runningBots = new HashSet<TransportData>();

		public TransportService(
			ILogger<TransportService> logger,
			IPlatformDatabase database,
			IServiceProvider serviceProvider,
			IOptions<TransportServiceOptions> options
			)
		{
			_logger = logger;
			_database = database;
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
					await StartBotsAsync();
					await StopBotsAsync();
					await Task.Delay(_options.ServiceIntervalMs, token);
				}
				catch (Exception ex)
				{
					_logger.LogCritical(ex, "Transport Service main loop error.");
				}
			}
		}

		//todo: этого тут не должно быть, нужо перенести в Counselor.Platform
		private async Task InitializeTransportsAsync()
		{
			var dbTransports = new List<Data.Entities.Transport>();

			foreach (var transport in _options.Transports)
			{
				if (string.IsNullOrEmpty(transport.SystemName)) continue;

				var dbTransport = await _database
					.Transports					
					.FirstOrDefaultAsync(x => x.Name.Equals(transport.SystemName)); //todo: нужно приводить в один регистр имя системы

				if (dbTransport == null)
				{
					await _database.Transports.AddAsync(
						new Data.Entities.Transport
						{
							Name = transport.SystemName,
							IsActive = transport.IsEnabled
						});

					await _database.SaveChangesAsync();
				}
				else
				{
					if (dbTransport.IsActive != transport.IsEnabled)
					{
						dbTransport.IsActive = transport.IsEnabled;
						_database.Transports.Update(dbTransport);
						await _database.SaveChangesAsync();
					}
				}

				dbTransports.Add(dbTransport);
			}

			var dbCommands = await _database.Commands
				.Include(x => x.Transport)
				.ToListAsync();			

			foreach (var type in TypeHelpers.GetTypesWithAttribute<InterpreterCommandAttribute>())
			{
				var transport = dbTransports.FirstOrDefault(x => type.FullName.Contains(x?.Name));

				if (!dbCommands.Exists(x => x.Name.Equals(type.Name) && transport?.Id == x.Transport?.Id))
				{
					var (isActive, parameters) = MergeAttributes(TypeHelpers.GetAppliedAttributes<InterpreterCommandAttribute>(type));

					await _database.Commands.AddAsync(
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

			await _database.SaveChangesAsync();
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

		public async Task StartBotsAsync()
		{
			var bots = await _database.Bots				
				.Where(x => x.BotState == BotState.Pending || x.BotState == BotState.Started)
				.Include(x => x.Transport)
				.Include(x => x.Owner)
				.Include(x => x.Script)
				.ToListAsync();

			if (bots.Any())
			{
				foreach (var bot in bots)
				{
					if (_runningBots.Any(x => x.Bot.Id == bot.Id)) continue;

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

					_database.Bots.Update(bot);
				}

				await _database.SaveChangesAsync();
			}
		}

		public async Task StopBotsAsync()
		{
			var lastExecution = DateTime.Now.AddMilliseconds(-1 * _options.ServiceIntervalMs);

			var stoppedBots = await _database.Bots
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

		public void EnsureValidity(Bot bot)
		{
			string invalidParameter = string.Empty;

			if (string.IsNullOrEmpty(bot.Script?.Data))
			{
				invalidParameter = nameof(bot.Script.Data);
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
										   _serviceProvider,
										   bot
										   ),
				CancellationToken.None
				),
			"Discord" => throw new NotImplementedException("Discord service is not implemented."),
			_ => throw new ArgumentOutOfRangeException(nameof(bot.Transport.Name), $"Unrecognized transport name. Transport name: {bot.Transport.Name}. BotId: {bot.Id}.")
		};
	}

	class TransportData
	{
		public Bot Bot { get; private set; }
		public IngoingServiceBase Transport { get; private set; }
		public CancellationToken CancellationToken { get; private set; }

		public TransportData(Bot bot, IngoingServiceBase transport, CancellationToken token)
		{
			if (bot == null) throw new ArgumentNullException(nameof(bot));
			if (transport == null) throw new ArgumentNullException(nameof(transport));

			Bot = bot;
			Transport = transport;
			CancellationToken = token;
		}

		public override int GetHashCode()
		{
			return Bot.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Bot))
				return false;
			else return Bot.Id == ((TransportData)obj).Bot.Id;
		}
	}
}
