using Counselor.Platform.Data.Entities;

namespace Counselor.Platform.Data
{
	public interface IAccessChecker
	{
		void EnsureAvailable(User user, AccessibleEntity entity);
		void EnsureAvailable(int userId, AccessibleEntity entity);
	}
}
