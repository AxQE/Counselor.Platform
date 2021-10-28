using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Counselor.Platform.Services;
using Counselor.Platform.Worker.Transport.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Transport
{
	class TransportService : BackgroundService, IDisposable
	{
		private readonly ILogger<TransportService> _logger;
		private readonly IPlatformDatabase _database;
		private readonly IServiceProvider _serviceProvider;
		private readonly HashSet<TransportData> _runningBots = new HashSet<TransportData>();
		private const int ServiceBotIntervalMs = 60000;

		public TransportService(
			ILogger<TransportService> logger,
			IPlatformDatabase database,
			IServiceProvider serviceProvider
			)
		{
			_logger = logger;
			_database = database;
			_serviceProvider = serviceProvider;
		}

		protected override async Task ExecuteAsync(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				try
				{
					await StartBotsAsync();
					await StopBotsAsync();
					await Task.Delay(ServiceBotIntervalMs, token);
				}
				catch (Exception ex)
				{
					_logger.LogCritical(ex, "Transport Service main loop error.");
				}
			}
		}

		private async Task StartBotsAsync()
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
					try
					{
						EnsureValidity(bot);

						var transportData = CreateTransport(bot);
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

		private async Task StopBotsAsync()
		{
			var stoppedBots = await _database.Bots
				.Where(x => x.BotState == BotState.Stopped && x.ModifiedOn > DateTime.Now.AddMilliseconds(-1 * ServiceBotIntervalMs))
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

		private void EnsureValidity(Bot bot)
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

			if (!string.IsNullOrEmpty(invalidParameter))
				throw new ArgumentException($"Bot is not valid. Invalid paramater: {invalidParameter}. BotId {bot.Id}.");
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
