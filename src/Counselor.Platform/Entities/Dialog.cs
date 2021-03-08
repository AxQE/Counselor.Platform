using Counselor.Platform.Core;
using System;
using System.Collections.Generic;

namespace Counselor.Platform.Entities
{
	public class Dialog : EntityBase, IDialog
	{
		public Guid Id { get; set; }
		public User User { get; set; }
		public IEnumerable<Message> Messages { get; set; }
		public int UserId { get => User.Id; }
		IEnumerable<IMessage> IDialog.Messages => Messages;
	}
}
