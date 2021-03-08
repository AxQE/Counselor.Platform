using System;
using System.Collections.Generic;
using System.Text;

namespace Counselor.Platform.Core
{
	interface IDialog
	{
		public Guid DialogId { get; set; }
		public int UserId { get; set; }
		public IEnumerable<IMessage> Messages { get; set; }
	}
}
