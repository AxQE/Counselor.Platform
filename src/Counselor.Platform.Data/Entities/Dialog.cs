using Counselor.Platform.Data.Entities.Enums;
using System.Collections.Generic;

namespace Counselor.Platform.Data.Entities
{
	public class Dialog : EntityBase
	{
		public User Client { get; set; }
		public List<Message> Messages { get; set; }
		public DialogState State { get; set; }
		public Message CurrentMessage { get; set; }
		public Bot Bot { get; set; }
		public int BotId { get; set; }
	}
}
