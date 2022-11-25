using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Exceptions;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Data
{
	public class AccessChecker : IAccessChecker
	{
		private readonly IPlatformDatabase _database;

		public AccessChecker(IPlatformDatabase database)
		{
			_database = database;
		}

		public void EnsureAvailable(User owner, AccessibleEntity entity)
		{
			ArgumentNullException.ThrowIfNull(owner, nameof(owner));
			EnsureAvailable(owner.Id, entity);
		}

		public void EnsureAvailable(int userId, AccessibleEntity entity)
		{
			ArgumentNullException.ThrowIfNull(entity, nameof(entity));

			if (entity.Owner.Id != userId)
			{
				throw new AccessDeniedException(userId, entity.OwnerId, entity.GetType().Name);
			}
		}

		public async Task EnsureAvailable(int userId, int entityId, Type entityType)
		{
			var entity = (AccessibleEntity)(await _database.GetAccessibleEntityAsync(entityId, entityType));

			if (entity.OwnerId != userId)
			{
				throw new AccessDeniedException(userId, entity.OwnerId, entity.GetType().Name);
			}
		}

		
	}
}
