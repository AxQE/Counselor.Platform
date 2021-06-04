using Counselor.Platform.Interpreter.Commands;
using Microsoft.Extensions.Options;

namespace Counselor.Platform.Worker.Transport.Discord.Commands
{
	class DiscordCommandFactory : TransportCommandFactory
	{
		public override string TransportName { get; }

		public DiscordCommandFactory(IOptions<DiscordOptions> options)
		{
			TransportName = options.Value.TransportSystemName;
		}
	}
}
