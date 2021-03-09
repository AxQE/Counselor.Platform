using Counselor.Platform.Database;
using Counselor.Platform.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Counselor.Platform.Repositories
{
	public class ConnectionsRepository
	{
		private readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();
		private readonly IPlatformDatabase _database;

		public ConnectionsRepository(IPlatformDatabase database)
		{
			_database = database;
		}

		public void AddConnection(int userId, string transport, string connectionId)
		{
			_connections.TryAdd(CreateKey(userId, transport), connectionId);
		}

		public async Task<string> GetConnectionIdAsync(int userId, string transport)
		{
			var key = CreateKey(userId, transport);
			
			if (!_connections.TryGetValue(key, out var connectionId))
			{
				var userTransport = await _database.UserTransports
							.Include(x => x.Transport)
							.FirstOrDefaultAsync(y => y.Transport.Name.Equals(transport));

				if (userTransport is null || string.IsNullOrEmpty(userTransport.TransportUserId)) 
					throw new EntityNotFoundException($"UserTransport not found. Transport: {transport}. User: {userId}.");

				connectionId = userTransport.TransportUserId;

				_connections.TryAdd(key, connectionId);
			}

			return connectionId;
		}

		private string CreateKey(int userId, string transport)
		{
			return $"{transport}_{userId}";
		}
	}
}
