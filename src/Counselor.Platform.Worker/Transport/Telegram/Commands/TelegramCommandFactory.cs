using Counselor.Platform.Interpreter.Commands;
using Microsoft.Extensions.Options;

namespace Counselor.Platform.Worker.Transport.Telegram.Commands
{
	class TelegramCommandFactory : TransportCommandFactory
	{
		public override string TransportName { get; }

		public TelegramCommandFactory(IOptions<TelegramOptions> options)
		{
			TransportName = options.Value.TransportSystemName;			
		}
	}
}
