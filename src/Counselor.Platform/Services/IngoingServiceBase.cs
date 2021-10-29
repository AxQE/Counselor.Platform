using Akka.Actor;
using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Services
{
	public abstract class IngoingServiceBase
	{
		private readonly ILogger<IngoingServiceBase> _logger;
		private readonly TransportOptions _options;
		private readonly IServiceProvider _serviceProvider;
		private readonly ActorSystem _actorSystem; //todo: перевести на акторы

		//todo: нужно время жизни
		private readonly ConcurrentDictionary<string, IBehaviorExecutor> _executors = new ConcurrentDictionary<string, IBehaviorExecutor>();

		public IngoingServiceBase(
			ILogger<IngoingServiceBase> logger,
			IOptions<TransportOptions> options,
			IServiceProvider serviceProvider
			)
		{
			_logger = logger;
			_options = options.Value;
			_serviceProvider = serviceProvider;
		}

		protected abstract Task StartTransportServiceAsync(CancellationToken cancellationToken);
		protected abstract Task StopTransportServiceAsync(CancellationToken cancellationToken);

		//todo: нужно посмотреть насколько этот метод вообще нужен, можно заинжектить outgoingservice и слать сообщения через него
		protected abstract Task SendMessageToTransportAsync(string connectionId, string payload, CancellationToken cancellationToken = default);

		protected async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (_options.IsEnabled)
			{
				try
				{
					_logger.LogInformation($"{_options.SystemName} service worker is starting.");
					while (!stoppingToken.IsCancellationRequested)
					{
						await StartTransportServiceAsync(stoppingToken);
						await Task.Delay(Timeout.Infinite, stoppingToken);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"{_options.SystemName} service worker got unprocessed error.");
				}
			}
			else
			{
				_logger.LogInformation($"{_options.SystemName} service worker is disabled.");
			}
		}

		protected async Task HandleMessageAsync(string connectionId, string username, string payload)
		{
			try
			{
				_logger.LogDebug($"Incoming message. Transport: {_options.SystemName}. ConnectionId: {connectionId}. Username: {username}. Payload: {payload}.");

				if (!_executors.TryGetValue(connectionId, out var begaviorExecutor))
				{
					begaviorExecutor = _serviceProvider.GetRequiredService<IBehaviorExecutor>();
					_executors.TryAdd(connectionId, begaviorExecutor);
				}

				await begaviorExecutor.RunBehaviorLogicAsync(connectionId, username, payload, _options.SystemName, _options.DialogName);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Behavior failed for {_options.SystemName}. ConnectionId: {connectionId}. Username: {username}. Payload: {payload}.");
			}
		}

		public async Task StartAsync(CancellationToken token)
		{
			if (_options.IsEnabled)
			{
				try
				{
					_logger.LogInformation($"{_options.SystemName} service worker is starting.");
					await StartTransportServiceAsync(token);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"{_options.SystemName} service worker got unprocessed error.");
				}
			}
			else
			{
				_logger.LogInformation($"{_options.SystemName} service worker is disabled.");
			}
		}

		public Task StopAsync()
		{
			return StopTransportServiceAsync(CancellationToken.None);
		}

		class ExecutorMeta
		{
			public IBehaviorExecutor Executor { get; init; }
			public DateTime CreatedOn { get; init; }
			public DateTime LastExecution { get; set; }
		}
	}
}
