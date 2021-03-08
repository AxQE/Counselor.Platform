using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Entities
{
	public class UserTransport : EntityBase
	{
		public int Id { get; set; }
		public Transport Transport { get; set; }
		public User User { get; set; }
	}
}
