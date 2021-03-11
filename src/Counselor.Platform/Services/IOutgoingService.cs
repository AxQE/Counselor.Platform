using System.Threading.Tasks;

namespace Counselor.Platform.Services
{
	public interface IOutgoingService
	{
		public string TransportSystemName { get; }
		Task SendAsync(string payload, int userId);
	}
}
