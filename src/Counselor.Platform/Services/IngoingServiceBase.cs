﻿using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Services
{
	public abstract class IngoingServiceBase
	{
		private readonly ILogger<IngoingServiceBase> _logger;
		private readonly TransportOptions _options;
		private readonly ServiceContext _serviceContext;
		private readonly IBehaviorExecutor _behaviorExecutor;

		protected IngoingServiceBase(
			ILogger<IngoingServiceBase> logger,
			IOptions<TransportOptions> options,
			IBehaviorExecutor behaviorExecutor,
			IPlatformDatabase database,
			ServiceContext serviceContext
			)
		{
			_logger = logger;
			_options = options.Value;
			_behaviorExecutor = behaviorExecutor;
			_serviceContext = serviceContext;

			using (database)
			{
				_behaviorExecutor.Initialize(_serviceContext.ScriptId, database);
			}
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
				_logger.LogDebug($"Incoming message. " +
					$"Transport: {_options.SystemName}. " +
					$"ConnectionId: {connectionId}. " +
					$"Username: {username}. " +
					$"Payload: {payload}. " +
					$"BotId: {_serviceContext.BotId}. " +
					$"OwnerId: {_serviceContext.OwnerId}.");

				await _behaviorExecutor.RunBehaviorLogicAsync(connectionId, username, payload, _serviceContext);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Message handling failed. " +
					$"Transport: {_options.SystemName}. " +
					$"ConnectionId: {connectionId}. " +
					$"Username: {username}. " +
					$"Payload: {payload}. " +
					$"BotId: {_serviceContext.BotId}. " +
					$"OwnerId: {_serviceContext.OwnerId}.");
			}
		}

		public async Task StartAsync(CancellationToken token)
		{
			if (_options.IsEnabled)
			{
				try
				{
					_logger.LogInformation($"{_options.SystemName} service worker is starting. BotId: {_serviceContext.BotId}.");
					await StartTransportServiceAsync(token);
				}
				catch (Exception ex)
				{
					_logger.LogCritical(ex, $"{_options.SystemName} service worker got unprocessed error. BotId: {_serviceContext.BotId}.");
				}
			}
			else
			{
				_logger.LogInformation($"{_options.SystemName} service worker is disabled. BotId: {_serviceContext.BotId}.");
			}
		}

		public Task StopAsync()
		{
			return StopTransportServiceAsync(CancellationToken.None);
		}
	}
}
