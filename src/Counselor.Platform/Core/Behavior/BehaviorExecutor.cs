using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Counselor.Platform.Interpreter;
using Counselor.Platform.Repositories.Interfaces;
using Counselor.Platform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorExecutor : IBehaviorExecutor
	{
		private readonly IInterpreter _interpreter;
		private readonly ILogger<BehaviorExecutor> _logger;
		private readonly IPlatformDatabase _database;
		private readonly IOutgoingServicePool _outgoingServicePool;
		private readonly IConnectionsRepository _connectionsRepository;
		private readonly IDialogsRepository _dialogsRepository;
		private readonly IBehaviorRepository _behaviorManager;
		private readonly SemaphoreSlim _executorSemaphore = new SemaphoreSlim(1, 1);

		private Transport _transport;
		private Dialog _dialog;
		private User _user;

		private static readonly Dictionary<string, Action> PredefinedBehaviorCommands = new Dictionary<string, Action>
		{
			{ "Next", null },
			{ "None", null },
			{ "Stop", null }
		};

		public BehaviorExecutor(
			IInterpreter interpreter, 
			ILogger<BehaviorExecutor> logger,
			IPlatformDatabase database,
			IOutgoingServicePool outgoingServicePool,
			IConnectionsRepository connectionsRepository,
			IDialogsRepository dialogsRepository,
			IBehaviorRepository behaviorRepository)
		{
			_interpreter = interpreter;
			_logger = logger;
			_database = database;
			_outgoingServicePool = outgoingServicePool;
			_connectionsRepository = connectionsRepository;
			_dialogsRepository = dialogsRepository;
			_behaviorManager = behaviorRepository;
		}

		public async Task RunBehaviorLogicAsync(string connectionId, string username, string payload, string transport, string dialogName)
		{
			try
			{
				await _executorSemaphore.WaitAsync();

				if (_transport == null) 
					_transport = await _database.Transports.FirstOrDefaultAsync(x => x.Name.Equals(transport));

				if (_user == null)
					_user = await FindOrCreateUserAsync(connectionId, username);

				_dialog = await _dialogsRepository.CreateOrUpdateDialogAsync(_database, _user, payload, MessageDirection.In, dialogName);

			}
			finally
			{
				_executorSemaphore.Release();
			}


			if (string.IsNullOrEmpty(_dialog.Name))
				throw new ArgumentNullException(nameof(_dialog.Name));

			var behaviorIterator = _behaviorManager.GetBehavior(_dialog.Name);

			while (behaviorIterator.Current() != null)
			{
				foreach (var step in behaviorIterator.Current())
				{
					if (step.IsActive)
					{
						await RunStepAsync(_database, _outgoingServicePool.Resolve(transport), _dialog, step);
					}
				}

				behaviorIterator.Next();
			}
		}

		private async Task RunStepAsync(IPlatformDatabase database, IOutgoingService outgoingService, Dialog dialog, BehaviorStep step)
		{
			try
			{
				//if (!(await _interpreter.Interpret(step.Condition, dialog, database)).GetTypedResult<bool>())
				//{
				//	return;
				//}

				if (step.Command != null)
				{
					if (!string.IsNullOrEmpty(step.Command?.Name) && PredefinedBehaviorCommands.TryGetValue(step.Command.Name, out var action))
					{
						action?.Invoke();
					}
					else
					{
						var result = await _interpreter.Interpret(step.Command, dialog, database);
					}
				}

				var response = await _interpreter.InsertEntityParameters(step.Response, dialog, database);

				await outgoingService.SendAsync(response, dialog.User.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Behavior step failed. Id: {step.Id}.");
				throw;
			}
		}

		private async Task<User> FindOrCreateUserAsync(string connectionId, string username)
		{
			var user =
				(await _database.UserTransports
					.Include(x => x.User)
					.FirstOrDefaultAsync(x => x.TransportUserId.Equals(connectionId)))?.User;

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

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
