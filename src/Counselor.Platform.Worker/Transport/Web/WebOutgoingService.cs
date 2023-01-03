using Counselor.Platform.Data.Options;
using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Repositories.Interfaces;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Transport.Web
{
	public class WebOutgoingService : OutgoingServiceBase
	{
		public WebOutgoingService(ILogger<OutgoingServiceBase> logger, IOptions<TransportOptions> options, IConnectionsRepository connections) 
			: base(logger, options, connections)
		{
		}

		public override Task SendAsync(ITransportCommand command)
		{
			throw new NotImplementedException();
		}

		protected override Task SendMessageToTransportAsync(string connectionId, string payload)
		{
			throw new NotImplementedException();
		}
	}
}
