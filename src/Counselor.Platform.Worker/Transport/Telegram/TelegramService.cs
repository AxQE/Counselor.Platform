using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Transport.Telegram
{
	public class TelegramService : IngoingServiceBase
	{
		private readonly ILogger<TelegramService> _logger;
		private readonly TelegramOptions _options;
		private readonly ITelegramBotClient _client;
		private readonly int _botId;

		public TelegramService(
			ILogger<TelegramService> logger,
			IOptions<TelegramOptions> options,
			IBehaviorExecutor behaviorExecutor,
			IPlatformDatabase database,
			ServiceContext serviceContext
			)
			: base(logger, options, behaviorExecutor, database, serviceContext)
		{
			_logger = logger;
			_options = options.Value;
			_botId = serviceContext.BotId;

			_client = new TelegramBotClient(_options.Token);
			_client.OnMessage += OnMessageAsync;
			_client.OnReceiveError += OnReceiveError;
		}

		private void OnReceiveError(object sender, global::Telegram.Bot.Args.ReceiveErrorEventArgs e)
		{
			_logger.LogError(e.ApiRequestException, $"Telegram api error. BotId: {_botId}.");
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
			_logger.LogInformation($"Telegram Service was stopped. BotId: {_botId}.");
			return Task.CompletedTask;
		}
	}
}
