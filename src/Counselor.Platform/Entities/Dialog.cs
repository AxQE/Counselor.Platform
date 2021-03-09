using System;
using System.Collections.Generic;

namespace Counselor.Platform.Entities
{
	public class Dialog : EntityBase
	{
		public Guid Id { get; set; }
		public User User { get; set; }
		public List<Message> Messages { get; set; }
	}
}
