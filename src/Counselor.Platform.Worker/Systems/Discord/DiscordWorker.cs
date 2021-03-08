using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Worker.Systems.Discord
{
	class DiscordWorker : BackgroundService
	{
		private readonly ILogger<DiscordWorker> _logger;
		private readonly DiscordOptions _options;		

		public DiscordWorker(ILogger<DiscordWorker> logger, IOptions<DiscordOptions> options)
		{
			_logger = logger;
			_options = options.Value;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (_options.IsEnabled)
			{
				_logger.LogInformation("Discord service worker is starting.");
			}
			else
			{
				_logger.LogInformation("Discord service worker is disabled.");
			}
		}
	}
}
