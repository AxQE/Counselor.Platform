using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Transport.Web
{
	public class WebService : IngoingServiceBase
	{
		public WebService(
			ILogger<IngoingServiceBase> logger,
			IOptions<TransportOptions> options,
			IBehaviorExecutor behaviorExecutor,
			IPlatformDatabase database,
			ServiceContext serviceContext
			)
			: base(logger, options, behaviorExecutor, database, serviceContext)
		{
		}

		protected override Task SendMessageToTransportAsync(string connectionId, string payload, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		protected override Task StartTransportServiceAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		protected override Task StopTransportServiceAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
