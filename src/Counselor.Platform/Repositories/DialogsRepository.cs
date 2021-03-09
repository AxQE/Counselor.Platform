using Counselor.Platform.Core;
using Counselor.Platform.Database;
using System.Collections.Concurrent;

namespace Counselor.Platform.Repositories
{
	public class DialogsRepository
	{
		private readonly IPlatformDatabase _database;
		private readonly ConcurrentDictionary<int, IDialog> _connections = new ConcurrentDictionary<int, IDialog>();

		public DialogsRepository(IPlatformDatabase database)
		{
			_database = database;
		}

		public void AddDialog(int userId, IDialog dialog)
		{
			_connections.TryAdd(userId, dialog);
		}

		public IDialog GetDialog(int userId)
		{
			return _connections.TryGetValue(userId, out var dialog) ? dialog : null;
		}
	}
}
