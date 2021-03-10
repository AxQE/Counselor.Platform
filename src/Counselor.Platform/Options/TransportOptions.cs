namespace Counselor.Platform.Options
{
	public abstract class TransportOptions
	{
		public abstract string TransportSystemName { get; }
		public abstract bool IsEnabled { get; set; }
		public abstract bool SendErrorReport { get; set; }
	}
}
