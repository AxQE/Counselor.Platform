using System.Threading.Tasks;

namespace Counselor.Platform.Repositories.Interfaces
{
	public interface IConnectionsRepository
	{
		void AddConnection(int userId, string transport, string connectionId);
		Task<string> GetConnectionIdAsync(int userId, string transport);
	}
}
