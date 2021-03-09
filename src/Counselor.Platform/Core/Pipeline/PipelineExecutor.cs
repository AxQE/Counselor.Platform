using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline
{
	public class PipelineExecutor : IPipelineExecutor
	{
		private readonly SortedSet<IPipelineStep> _processingSteps = new SortedSet<IPipelineStep>();
		private readonly ILogger<PipelineExecutor> _logger;
		private readonly IPlatformDatabase _database;
		private readonly ConnectionsRepository _connections;
		private readonly DialogsRepository _dialogs;

		private Transport _transport;
		private Dialog _dialog;
		private User _user;

		public PipelineExecutor(
			ILogger<PipelineExecutor> logger,
			IPlatformDatabase database,
			ConnectionsRepository connections,
			DialogsRepository dialogs
			)
		{
			_logger = logger;
			_database = database;
			_connections = connections;
			_dialogs = dialogs;
		}

		public async Task<PipelineResult> RunAsync(string connectionId, string username, string payload, string transport)
		{
			var result = new PipelineResult();

			try
			{
				if (_transport == null)
				{
					_transport = await _database.Transports.FirstOrDefaultAsync(x => x.Name.Equals(transport));
				}

				if (_user == null)
				{
					_user = await FindOrCreateUserAsync(connectionId, username);
				}

				_dialog = await _dialogs.CreateOrUpdateDialogAsync(_database, _user, payload);

				foreach (var step in _processingSteps)
				{
					await step.ExecuteAsync(_dialog);
				}				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Message pipeline failed. DialogId: {_dialog.Id}. UserId: {_dialog.User.Id}.");
				result.SuccessfullyCompleted = false;
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

				await _database.SaveChangesAsync();
				_logger.LogInformation($"New user just created. Username: {username}. Transport: {_transport.Name}.");
			}

			_connections.AddConnection(user.Id, _transport.Name, connectionId);

			return user;
		}
	}
}
