using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Transport.Discord
{
	class DiscordService : IngoingServiceBase
	{
		private readonly ILogger<DiscordService> _logger;
		private readonly DiscordOptions _options;

		public DiscordService(
			ILogger<DiscordService> logger,
			IOptions<DiscordOptions> options,			
			IBehaviorExecutor behaviorExecutor,
			IPlatformDatabase database,
			ServiceContext serviceContext
			)
			: base(logger, options, behaviorExecutor, database, serviceContext)
		{
			_logger = logger;
			_options = options.Value;
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
