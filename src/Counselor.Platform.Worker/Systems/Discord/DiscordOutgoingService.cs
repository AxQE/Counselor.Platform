using Counselor.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Systems.Discord
{
	class DiscordOutgoingService : IOutgoingService
	{
		public string TransportSystemName => "Discord";

		public Task SendAsync(IMessage message, int userId)
		{
			throw new NotImplementedException();
		}
	}
}
