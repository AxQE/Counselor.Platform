namespace Counselor.Platform.Data.Entities
{
	public abstract class AccessibleEntity : EntityBase
	{
		public User Owner { get; set; }
		public int OwnerId { get; set; }
	}
}
