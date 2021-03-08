namespace Counselor.Platform.Core
{
	interface IOutgoingServicesPool
	{
		IOutgoingService Resolve(IMessage message);
		void Register(IOutgoingService outgoingService);
	}
}
