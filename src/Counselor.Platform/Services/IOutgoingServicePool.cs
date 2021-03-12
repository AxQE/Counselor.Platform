namespace Counselor.Platform.Services
{
	public interface IOutgoingServicePool
	{
		IOutgoingService Resolve(string transport);
	}
}
