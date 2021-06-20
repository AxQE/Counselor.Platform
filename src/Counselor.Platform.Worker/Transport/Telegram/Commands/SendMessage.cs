using Counselor.Platform.Interpreter.Commands;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Transport.Telegram.Commands
{
	class SendMessage : ITransportCommand
	{
		public object Parameter { get; set; }
		public string ConnectionId { get; set; }

		public async Task ExecuteAsync(object transportClient)
		{
			var client = transportClient as TelegramBotClient;
			if (client == null)
				throw new InvalidCastException($"The type {transportClient.GetType()} is not compatible with TelegramBotClient.");

			var message = Parameter as string;
			if (string.IsNullOrEmpty(message))
				throw new InvalidCastException("Command parameter must be non null string.");

			await client.SendTextMessageAsync(long.Parse(ConnectionId), message);
		}
	}
}
