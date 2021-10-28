using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Repositories.Interfaces;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Transport.Telegram
{
	class TelegramOutgoingService : OutgoingServiceBase
	{
		private readonly ILogger<TelegramOutgoingService> _logger;
		private readonly TelegramOptions _options;
		private readonly TelegramBotClient _client;

		public TelegramOutgoingService(
			ILogger<TelegramOutgoingService> logger,
			IOptions<TelegramOptions> options,
			IConnectionsRepository connections
			)
			: base(logger, options, connections)
		{
			_logger = logger;
			_options = options.Value;

			_client = new TelegramBotClient(_options.Token);
		}

		public override Task SendAsync(ITransportCommand command)
		{
			return command.ExecuteAsync(_client);
		}

		protected override async Task SendMessageToTransportAsync(string connectionId, string message)
		{
			try
			{
				await _client.SendTextMessageAsync(long.Parse(connectionId), message);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error during send message to telegram.");
				throw;
			}
		}
	}
}
