using Counselor.Platform.Data.Entities.Enums;

namespace Counselor.Platform.Data.Entities
{
	public class Message : EntityBase
	{
		public string Payload { get; set; }
		public MessageDirection Direction { get; set; }
	}
}
