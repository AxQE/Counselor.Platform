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
		public string SystemName => throw new NotImplementedException();

		public Task Send(IMessage message)
		{
			throw new NotImplementedException();
		}
	}
}
