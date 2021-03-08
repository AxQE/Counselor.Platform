using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Systems.Telegram
{
	class TelegramWorker : BackgroundService
	{
		private readonly ILogger<TelegramWorker> _logger;
		private readonly TelegramOptions _options;
		private readonly TelegramBotClient _client;

		public TelegramWorker(ILogger<TelegramWorker> logger, IOptions<TelegramOptions> options)
		{
			_logger = logger;
			_options = options.Value;

			_client = new TelegramBotClient(_options.Token);
			BotInitialize();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (_options.IsEnabled)
			{
				_logger.LogInformation("Telegram service worker is starting.");
				while (!stoppingToken.IsCancellationRequested)
				{
					_client.StartReceiving();
					await Task.Delay(Timeout.Infinite, stoppingToken);
				}
			}
			else
			{
				_logger.LogInformation("Telegram service worker is disabled.");
			}
		}

		private void BotInitialize()
		{
			_client.OnMessage += OnMessage;
			_client.OnReceiveError += OnReceiveError;
		}

		private void OnReceiveError(object sender, global::Telegram.Bot.Args.ReceiveErrorEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void OnMessage(object sender, global::Telegram.Bot.Args.MessageEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Message.Text))
			{

			}			
		}		
	}
}
