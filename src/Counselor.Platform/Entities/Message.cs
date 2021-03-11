using Counselor.Platform.Entities.Enums;

namespace Counselor.Platform.Entities
{
	public class Message : EntityBase
	{
		public string Payload { get; set; }
		public MessageDirection Direction { get; set; }
	}
}
