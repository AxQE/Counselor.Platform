using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Exceptions;
using System;

namespace Counselor.Platform.Data
{
	public static class AccessChecker
	{
		public static void EnsureAvailable(User owner, AccessibleEntity entity)
		{
			ArgumentNullException.ThrowIfNull(owner, nameof(owner));
			EnsureAvailable(owner.Id, entity);
		}

		public static void EnsureAvailable(int userId, AccessibleEntity entity)
		{
			ArgumentNullException.ThrowIfNull(entity, nameof(entity));

			if (entity.OwnerId != userId)
			{
				throw new AccessDeniedException(userId, entity.OwnerId, entity.GetType().Name);
			}
		}
	}
}
