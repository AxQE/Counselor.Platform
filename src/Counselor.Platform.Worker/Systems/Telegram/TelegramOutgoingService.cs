using Counselor.Platform.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Systems.Telegram
{
	class TelegramOutgoingService : IOutgoingService
	{
		public string SystemName => "Telegram";
		private readonly TelegramOptions _options;
		private readonly TelegramBotClient _client;

		public TelegramOutgoingService(ILogger<TelegramWorker> logger, IOptions<TelegramOptions> options)
		{
			_options = options.Value;

			_client = new TelegramBotClient(_options.Token);
		}

		public async Task Send(IMessage message)
		{
			//_client.SendTextMessageAsync();
		}
	}
}
