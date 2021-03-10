using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Transport.Discord
{
	class DiscordWorker : IngoingServiceBase
	{
		private readonly ILogger<DiscordWorker> _logger;
		private readonly DiscordOptions _options;		

		public DiscordWorker(
			ILogger<DiscordWorker> logger, 
			IOptions<DiscordOptions> options,
			IServiceProvider serviceProvider
			)
			: base(logger, options, serviceProvider)
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
	}
}
