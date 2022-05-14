using System.ComponentModel.DataAnnotations;

namespace Counselor.Platform.Api.Models.Dto
{
	public class AuthDto
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
