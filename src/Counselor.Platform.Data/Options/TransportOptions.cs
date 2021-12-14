namespace Counselor.Platform.Data.Options
{
	public class TransportOptions
	{
		public virtual string SystemName { get; set; }
		public virtual bool IsEnabled { get; set; }
		public virtual bool SendErrorReport { get; set; }
		public virtual string DialogName { get; set; }
	}
}
