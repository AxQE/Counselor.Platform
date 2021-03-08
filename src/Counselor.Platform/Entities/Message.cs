using Counselor.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Entities
{
	public class Message : EntityBase, IMessage
	{
		public Guid Id { get; set; }
		public string Payload { get; set; }
	}
}
