namespace Counselor.Platform.Data.Entities
{
	public class UserTransport : EntityBase
	{
		public Transport Transport { get; set; }
		public User User { get; set; }
		public string TransportUserId { get; set; }
	}
}
