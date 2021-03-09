using System;
using System.Collections.Generic;

namespace Counselor.Platform.Core
{
	public interface IDialog
	{
		public Guid Id { get; set; }
		public int UserId { get; }
		public List<IMessage> Messages { get; }
	}
}
