namespace Counselor.Platform.Services
{
	internal interface IOutgoingServicePool
	{
		IOutgoingService Resolve(string transport);
	}
}
