using Counselor.Platform.Interpreter.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Counselor.Platform.Worker.Transport.Telegram.Commands
{
	class SendMessage : ITransportCommand
	{
		public Task ExecuteAsync<TelegramBotClient>(TelegramBotClient transportClient)
		{
			throw new NotImplementedException();
		}
	}
}
