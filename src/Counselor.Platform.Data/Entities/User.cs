using System;

namespace Counselor.Platform.Data.Entities
{
	public class User : EntityBase
	{
		public string Username { get; set; }
		public DateTime? LastActivity { get; set; }
	}
}
