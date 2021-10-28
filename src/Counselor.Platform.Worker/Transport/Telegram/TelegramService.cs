using Counselor.Platform.Data.Entities;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Transport.Telegram
{
	class TelegramService : IngoingServiceBase
	{
		private readonly ILogger<TelegramService> _logger;
		private readonly TelegramOptions _options;
		private readonly TelegramBotClient _client;
		private readonly Bot _bot;

		public TelegramService(
			ILogger<TelegramService> logger,
			IOptions<TelegramOptions> options,
			IServiceProvider serviceProvider,
			Bot bot
			)
			: base(logger, options, serviceProvider)
		{
			_logger = logger;
			_options = options.Value;
			_bot = bot;

			_client = new TelegramBotClient(_options.Token);
			_client.OnMessage += OnMessageAsync;
			_client.OnReceiveError += OnReceiveErrorAsync;
		}

		private async void OnReceiveErrorAsync(object sender, global::Telegram.Bot.Args.ReceiveErrorEventArgs e)
		{
			throw new NotImplementedException();
		}

		private async void OnMessageAsync(object sender, global::Telegram.Bot.Args.MessageEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Message.Text))
			{
				await HandleMessageAsync(e.Message.Chat.Id.ToString(), e.Message.Chat.Username, e.Message.Text);
			}
		}

		protected override Task StartTransportServiceAsync(CancellationToken cancellationToken)
		{
			_client.StartReceiving(cancellationToken: cancellationToken);
			return Task.CompletedTask;
		}

		protected override Task SendMessageToTransportAsync(string connectionId, string payload, CancellationToken cancellationToken = default)
		{
			return _client.SendTextMessageAsync(long.Parse(connectionId), payload, cancellationToken: cancellationToken);
		}

		protected override Task StopTransportServiceAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
