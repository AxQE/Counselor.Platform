using System.ComponentModel.DataAnnotations;

namespace Counselor.Platform.Api.Models.Requests
{
	public class UserCreateRequest
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
