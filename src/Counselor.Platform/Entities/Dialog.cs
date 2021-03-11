using Counselor.Platform.Entities.Enums;
using System.Collections.Generic;

namespace Counselor.Platform.Entities
{
	public class Dialog : EntityBase
	{
		public User User { get; set; }
		public List<Message> Messages { get; set; }
		public DialogState State { get; set; }
		public Message CurrentMessage { get; set; }
	}
}
