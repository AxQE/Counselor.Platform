using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Entities
{
	public class Dialog : EntityBase
	{
		public User User { get; set; }
		public IEnumerable<Message> Messages { get; set; }
	}
}
