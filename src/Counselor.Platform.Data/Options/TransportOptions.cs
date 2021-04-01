namespace Counselor.Platform.Data.Options
{
	public abstract class TransportOptions
	{
		public abstract string TransportSystemName { get; }
		public abstract bool IsEnabled { get; set; }
		public abstract bool SendErrorReport { get; set; }
		public abstract string DialogName { get; set; }
	}
}
