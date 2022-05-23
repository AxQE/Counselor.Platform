using System.ComponentModel.DataAnnotations;

namespace Counselor.Platform.Api.Models.Requests
{
	public class AuthRequest
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
