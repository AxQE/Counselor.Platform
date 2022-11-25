using System;

namespace Counselor.Platform.Data.Exceptions
{
	public class AccessDeniedException : Exception
	{
		public int EntityId { get; }
		public int UserId { get; }
		public string EntityType { get; }

		public override string Message => $"User ({UserId}) has no access to entity ({EntityType}:{EntityId}).";

		public AccessDeniedException(int userId, int entityId, string entityType)
		{
			if (string.IsNullOrEmpty(entityType))
			{
				throw new ArgumentNullException(nameof(entityType));
			}

			UserId = userId;
			EntityId = entityId;
			EntityType = entityType;
		}
	}
}
