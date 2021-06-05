using Counselor.Platform.Interpreter.Commands;
using Microsoft.Extensions.Options;

namespace Counselor.Platform.Worker.Transport.Discord.Commands
{
	class DiscordCommandFactory : TransportCommandFactory
	{
		public DiscordCommandFactory(IOptions<DiscordOptions> options)
			: base(options.Value.TransportSystemName)
		{			
		}
	}
}
