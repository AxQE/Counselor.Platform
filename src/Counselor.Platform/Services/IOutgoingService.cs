using System.Threading.Tasks;

namespace Counselor.Platform.Services
{
	public interface IOutgoingService
	{
		string TransportSystemName { get; }
		Task SendAsync(string payload, int userId);
	}
}
