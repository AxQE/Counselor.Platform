using Counselor.Platform.Interpreter.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Transport.Discord.Commands
{
	class DiscordCommandFactory : ITransportCommandFactory
	{
		public string TransportName => throw new NotImplementedException();

		public ITransportCommand CreateCommand(string identificator)
		{
			throw new NotImplementedException();
		}

		public Task<ITransportCommand> CreateCommandAsync(string identificator)
		{
			throw new NotImplementedException();
		}
	}
}
