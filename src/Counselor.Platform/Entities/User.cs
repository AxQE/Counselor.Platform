using System;

namespace Counselor.Platform.Entities
{
	public class User : EntityBase
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public DateTime? LastActivity { get; set; }
	}
}
