namespace Counselor.Platform.Entities
{
	public class UserTransport : EntityBase
	{
		public int Id { get; set; }
		public Transport Transport { get; set; }
		public User User { get; set; }
		public string TransportUserId { get; set; }
	}
}
