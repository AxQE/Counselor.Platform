using Akka.Actor;
using Counselor.Platform.Core.Pipeline;
using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Options;
using Microsoft.EntityFrameworkCore;
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
		private readonly ActorSystem _actorSystem; //todo: перевести PipelineExecutor на акторы

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
			var transport = database
				.Transports
				.AsNoTracking()
				.FirstOrDefault(x => x.Name.Equals(_options.TransportSystemName)); //todo: нужно приводить в один регистр имя системы

			if (transport is null)
			{
				transport = new Transport
				{
					Name = _options.TransportSystemName
				};

				database.Transports.Add(transport);
				database.SaveChanges();
			}
		}

		protected abstract Task StartTransportServiceAsync(CancellationToken cancellationToken);

		//todo: нужно посмотреть насколько этот метод вообще нужен, можно заинжектить outgoingservice и слать сообщения через него
		protected abstract Task SendMessageToTransportAsync(string connectionId, string payload, CancellationToken cancellationToken = default);

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
				
				var result = await pipelineExecutor.RunAsync(connectionId, username, payload, _options.TransportSystemName, _options.DialogName);

				if (_options.SendErrorReport && !result.SuccessfullyCompleted)
				{
					//todo: добавить подробности ошибки (они лежат в результате), возможно делить ошибки на классы и те которые потенциально могут быть исправлены пользователем репортить
					await SendMessageToTransportAsync(connectionId, "Во время обработки сообщения произошла ошибка. Обратитесь к системному администратору.");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Pipeline failed for {_options.TransportSystemName}. ConnectionId: {connectionId}. Username: {username}. Payload: {payload}.");
			}
		}

		class ExecutorMeta
		{
			public IPipelineExecutor Executor { get; init; }
			public DateTime CreatedOn { get; init; }
			public DateTime LastExecution { get; set; }
		}
	}
}
