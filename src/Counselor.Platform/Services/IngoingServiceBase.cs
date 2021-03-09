using Counselor.Platform.Core;
using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Options;
using Counselor.Platform.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Services
{
	public abstract class IngoingServiceBase : BackgroundService
	{
		private readonly Transport _transport;
		private readonly ILogger<IngoingServiceBase> _logger;
		private readonly TransportOptions _options;
		private readonly IPipelineExecutor _pipelineExecutor;
		private readonly IPlatformDatabase _database;
		private readonly ConnectionsRepository _connections;
		private readonly DialogsRepository _dialogs;

		protected abstract Action StartTransport { get; }


		public IngoingServiceBase(
			ILogger<IngoingServiceBase> logger,
			IOptions<TransportOptions> options,
			IPipelineExecutor pipelineExecutor,
			IPlatformDatabase database,
			ConnectionsRepository connections,
			DialogsRepository dialogs
			)
		{
			_logger = logger;
			_options = options.Value;
			_pipelineExecutor = pipelineExecutor;
			_database = database;
			_connections = connections;
			_dialogs = dialogs;

			_transport = _database.Transports.FirstOrDefault(x => x.Name.Equals(_options.TransportSystemName));

			if (_transport is null)
			{
				_transport = new Transport
				{
					Name = _options.TransportSystemName
				};

				_database.SaveChanges();
			}
		}

		protected override sealed async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (_options.IsEnabled)
			{
				try
				{
					_logger.LogInformation($"{_options.TransportSystemName} service worker is starting.");
					while (!stoppingToken.IsCancellationRequested)
					{
						StartTransport();
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
				await _pipelineExecutor.RunAsync(await FindOrCreateDialogAsync(connectionId, username, payload));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Pipeline failed for {_options.TransportSystemName}. ConnectionId: {connectionId}. Username: {username}. Payload: {payload}.");
			}
		}

		protected virtual async Task<User> FindOrCreateUserAsync(string connectionId, string username)
		{
			//todo: хранить пользователя в кэше

			var user =
				(await _database.UserTransports
					.Include(x => x.User)
					.FirstOrDefaultAsync(x => x.Transport.Id == _transport.Id))?.User;

			if (user is null)
			{
				_database.UserTransports.Add(
					new UserTransport
					{
						Transport = _transport,
						User = new User
						{
							Username = username
						},
						TransportUserId = connectionId
					});
				
				await _database.SaveChangesAsync();
				_logger.LogInformation($"New user just created. Username: {username}. Transport: {_options.TransportSystemName}.");
			}

			_connections.AddConnection(user.Id, _options.TransportSystemName, connectionId);

			return user;
		}

		protected virtual async Task<IDialog> FindOrCreateDialogAsync(string connectionId, string username, string payload)
		{
			var user = await FindOrCreateUserAsync(connectionId, username);
			return await _dialogs.GetDialogAsync(user, payload);			
		}
	}
}
