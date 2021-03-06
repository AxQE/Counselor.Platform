using System;

namespace Counselor.Platform.Data.Entities
{
	public class User : EntityBase
	{
		public string Username { get; set; } //todo: пользователь идентифицируется по имени, которое может быть изменено, нужно тянуть id
		public DateTime? LastActivity { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public string Email { get; set; }
	}
}
