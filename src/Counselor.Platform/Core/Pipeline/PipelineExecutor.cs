using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Entities.Enums;
using Counselor.Platform.Repositories;
using Counselor.Platform.Services;
using Counselor.Platform.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline
{
	internal class PipelineExecutor : IPipelineExecutor
	{
		private readonly SortedSet<IPipelineStep> _processingSteps = new(new PipelineStepComparer());
		private readonly ILogger<PipelineExecutor> _logger;
		private readonly IPlatformDatabase _database;
		private readonly IServiceProvider _serviceProvider;
		private readonly IOutgoingServicePool _outgoingServicePool;
		private readonly ConnectionsRepository _connectionsRepository;
		private readonly DialogsRepository _dialogsRepository;
		private readonly SemaphoreSlim _executorSemaphore = new SemaphoreSlim(1, 1);

		private Transport _transport;
		private Dialog _dialog;
		private User _user;

		public PipelineExecutor(
			ILogger<PipelineExecutor> logger,
			IPlatformDatabase database,
			IServiceProvider serviceProvider,
			IOutgoingServicePool outgoingServicePool,
			ConnectionsRepository connectionsRepository,
			DialogsRepository dialogsRepository
			)
		{
			_logger = logger;
			_database = database;
			_outgoingServicePool = outgoingServicePool;
			_connectionsRepository = connectionsRepository;
			_dialogsRepository = dialogsRepository;
			_serviceProvider = serviceProvider;

			foreach (var serviceType in TypeHelpers.GetTypeImplementations<IPipelineStep>())
			{
				var service = _serviceProvider.GetService(serviceType) as IPipelineStep;
				if (service != null)
					_processingSteps.Add(service);
				else
					_logger.LogWarning($"Was retrieved null for service type: {serviceType}.");
			}
		}

		public void Dispose()
		{
			//todo: после введения времени жизни, пока пусть так живет
		}

		public async Task<PipelineResult> RunAsync(string connectionId, string username, string payload, string transport, string dialog)
		{
			var result = new PipelineResult();

			try
			{
				await _executorSemaphore.WaitAsync();

				if (_transport == null)
				{
					_transport = await _database.Transports.FirstOrDefaultAsync(x => x.Name.Equals(transport));
				}

				if (_user == null)
				{
					_user = await FindOrCreateUserAsync(connectionId, username);
				}

				_dialog = await _dialogsRepository.CreateOrUpdateDialogAsync(_database, _user, payload, MessageDirection.In, dialog);

				foreach (var step in _processingSteps)
				{
					await step.ExecuteAsync(_database, _outgoingServicePool.Resolve(transport), _dialog, transport);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Message pipeline failed. DialogId: {_dialog.Id}. UserId: {_dialog.User.Id}.");
				result.SuccessfullyCompleted = false;
			}
			finally
			{
				_executorSemaphore.Release();
			}

			return result;
		}

		private async Task<User> FindOrCreateUserAsync(string connectionId, string username)
		{
			var user =
				(await _database.UserTransports
					.Include(x => x.User)
					.FirstOrDefaultAsync(x => x.Transport.Id == _transport.Id))?.User;

			if (user is null)
			{
				user = new User
				{
					Username = username
				};

				_database.UserTransports.Add(
					new UserTransport
					{
						Transport = _transport,
						User = user,
						TransportUserId = connectionId
					});

				_logger.LogInformation($"New user just created. Username: {username}. Transport: {_transport.Name}.");
			}

			user.LastActivity = DateTime.Now;
			await _database.SaveChangesAsync();

			_connectionsRepository.AddConnection(user.Id, _transport.Name, connectionId);

			return user;
		}
	}
}
