using System;

namespace Counselor.Platform.Data.Exceptions
{
	public class InconsistentEntityException : Exception
	{
		public int EntityId { get; }
		public string EntityType { get; }
		public string EntityAttribute { get; }

		public override string Message => $"Entity {EntityType}:{EntityId} attribute {EntityAttribute} is inconsistent.";

		public InconsistentEntityException(int entityId, string entityType, string entityAttribute)
		{
			if (string.IsNullOrEmpty(entityType))
			{
				throw new ArgumentNullException(nameof(entityType));
			}

			if (string.IsNullOrEmpty(entityAttribute))
			{
				throw new ArgumentNullException(nameof(entityAttribute));
			}

			EntityId = entityId;
			EntityType = entityType;
			EntityAttribute = entityAttribute;
		}
	}
}
