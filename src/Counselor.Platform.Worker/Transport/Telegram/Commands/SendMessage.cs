using Counselor.Platform.Interpreter.Commands;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Transport.Telegram.Commands
{
	class SendMessage : ITransportCommand
	{
		public async Task ExecuteAsync(object transportClient, string connectionId, object commandParameter)
		{
			var client = transportClient as TelegramBotClient;
			if (client == null)
				throw new InvalidCastException($"The type {transportClient.GetType()} is not compatible with TelegramBotClient.");

			var message = commandParameter as string;
			if (string.IsNullOrEmpty(message))
				throw new ArgumentException("Command parameter must be non null string.");

			await client.SendTextMessageAsync(long.Parse(connectionId), message);
		}
	}
}
