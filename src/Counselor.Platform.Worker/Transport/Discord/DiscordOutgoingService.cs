using Counselor.Platform.Repositories.Interfaces;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Transport.Discord
{
	class DiscordOutgoingService : OutgoingServiceBase
	{
		private readonly ILogger<DiscordOutgoingService> _logger;
		private readonly DiscordOptions _options;

		public DiscordOutgoingService(
			ILogger<DiscordOutgoingService> logger,
			IOptions<DiscordOptions> options,
			IConnectionsRepository connections
			)
			: base(logger, options, connections)
		{
			_logger = logger;
			_options = options.Value;
		}

		protected override async Task SendMessageToTransportAsync(string connection, string message)
		{
			throw new NotImplementedException();
		}
	}
}
