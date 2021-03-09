using Counselor.Platform.Entities;

namespace Counselor.Platform.Services
{
	interface IOutgoingServicePool
	{
		IOutgoingService Resolve(Message message);
		void Register(IOutgoingService outgoingService);
	}
}
