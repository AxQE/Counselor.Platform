using System.Threading.Tasks;

namespace Counselor.Platform.Core
{
	public interface IOutgoingService
	{
		public string SystemName { get; }
		Task Send(IMessage message);
	}
}
