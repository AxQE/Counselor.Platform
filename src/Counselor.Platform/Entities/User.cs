using System;

namespace Counselor.Platform.Entities
{
	public class User : EntityBase
	{
		public string Username { get; set; }
		public DateTime? LastActivity { get; set; }
	}
}
