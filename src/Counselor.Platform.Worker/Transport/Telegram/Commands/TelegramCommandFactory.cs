using Counselor.Platform.Interpreter.Commands;
using Microsoft.Extensions.Options;

namespace Counselor.Platform.Worker.Transport.Telegram.Commands
{
	class TelegramCommandFactory : TransportCommandFactory
	{
		public TelegramCommandFactory(IOptions<TelegramOptions> options)
			: base(options.Value.SystemName)
		{
		}
	}
}
