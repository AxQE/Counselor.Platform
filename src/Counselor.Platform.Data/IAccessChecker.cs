using Counselor.Platform.Data.Entities;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Data
{
	public interface IAccessChecker
	{
		void EnsureAvailable(User user, AccessibleEntity entity);
		void EnsureAvailable(int userId, AccessibleEntity entity);
		Task EnsureAvailable(int userId, int entityId, Type entityType);
	}
}
