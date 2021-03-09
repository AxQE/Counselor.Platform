using Counselor.Platform.Core.Pipeline;
using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Services
{
	public abstract class IngoingServiceBase : BackgroundService
	{		
		private readonly ILogger<IngoingServiceBase> _logger;
		private readonly TransportOptions _options;
		private readonly IServiceProvider _serviceProvider;

		//todo: нужно время жизни
		private readonly ConcurrentDictionary<string, IPipelineExecutor> _executors = new ConcurrentDictionary<string, IPipelineExecutor>();		

		public IngoingServiceBase(
			ILogger<IngoingServiceBase> logger,
			IOptions<TransportOptions> options,
			IServiceProvider serviceProvider			
			)
		{
			_logger = logger;
			_options = options.Value;
			_serviceProvider = serviceProvider;

			var database = _serviceProvider.GetRequiredService<IPlatformDatabase>();
			var transport = database.Transports.FirstOrDefault(x => x.Name.Equals(_options.TransportSystemName));

			if (transport is null)
			{
				transport = new Transport
				{
					Name = _options.TransportSystemName
				};

				database.SaveChanges();
			}
		}

		protected abstract Task StartTransportServiceAsync(CancellationToken cancellationToken);

		protected override sealed async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (_options.IsEnabled)
			{
				try
				{
					_logger.LogInformation($"{_options.TransportSystemName} service worker is starting.");
					while (!stoppingToken.IsCancellationRequested)
					{
						await StartTransportServiceAsync(stoppingToken);
						await Task.Delay(Timeout.Infinite, stoppingToken);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"{_options.TransportSystemName} service worker got unprocessed error.");
				}
			}
			else
			{
				_logger.LogInformation($"{_options.TransportSystemName} service worker is disabled.");
			}
		}

		protected async Task HandleMessageAsync(string connectionId, string username, string payload)
		{
			try
			{
				_logger.LogDebug($"Incoming message. Transport: {_options.TransportSystemName}. ConnectionId: {connectionId}. Username: {username}. Payload: {payload}.");

				if (!_executors.TryGetValue(connectionId, out var pipelineExecutor))
				{
					pipelineExecutor = _serviceProvider.GetRequiredService<IPipelineExecutor>();
					_executors.TryAdd(connectionId, pipelineExecutor);
				}
				
				await pipelineExecutor.RunAsync(connectionId, username, payload, _options.TransportSystemName);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Pipeline failed for {_options.TransportSystemName}. ConnectionId: {connectionId}. Username: {username}. Payload: {payload}.");
			}
		}
	}
}
