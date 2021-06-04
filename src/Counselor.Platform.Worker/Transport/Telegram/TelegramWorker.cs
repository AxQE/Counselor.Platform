using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Transport.Telegram
{
	class TelegramWorker : IngoingServiceBase
	{
		private readonly ILogger<TelegramWorker> _logger;
		private readonly TelegramOptions _options;
		private readonly TelegramBotClient _client;

		public TelegramWorker(
			ILogger<TelegramWorker> logger,
			IOptions<TelegramOptions> options,
			IServiceProvider serviceProvider,
			IEnumerable<ITransportCommandFactory> factories
			)
			: base(logger, options, serviceProvider)
		{
			_logger = logger;
			_options = options.Value;

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

		protected override async Task StartTransportServiceAsync(CancellationToken cancellationToken)
		{
			_client.StartReceiving(cancellationToken: cancellationToken);
		}

		protected override async Task SendMessageToTransportAsync(string connectionId, string payload, CancellationToken cancellationToken = default)
		{
			await _client.SendTextMessageAsync(long.Parse(connectionId), payload, cancellationToken: cancellationToken);
		}
	}
}
