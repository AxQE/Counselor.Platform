namespace Counselor.Platform.Data.Options
{
	public abstract class TransportOptions
	{
		public abstract string SystemName { get; }
		public virtual bool IsEnabled { get; set; }
		public virtual bool SendErrorReport { get; set; }
		public virtual string DialogName { get; set; }
	}
}
