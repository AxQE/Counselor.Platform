using Counselor.Platform.Entities;

namespace Counselor.Platform.Services
{
	internal interface IOutgoingServicePool
	{
		IOutgoingService Resolve(Message message);
		IOutgoingService Resolve(string transport);
		void Register(IOutgoingService outgoingService);
	}
}
