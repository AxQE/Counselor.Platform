using Counselor.Platform.Data.Entities.Enums;
using System.Collections.Generic;

namespace Counselor.Platform.Data.Entities
{
	public class Dialog : EntityBase
	{
		public User User { get; set; }
		public List<Message> Messages { get; set; }
		public DialogState State { get; set; }
		public Message CurrentMessage { get; set; }
		public string Name { get; set; }
	}
}
