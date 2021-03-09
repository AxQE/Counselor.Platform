using System.Threading.Tasks;

namespace Counselor.Platform.Core
{
	public interface IOutgoingService
	{
		public string TransportSystemName { get; }
		Task SendAsync(IMessage message, int userId);
	}
}
