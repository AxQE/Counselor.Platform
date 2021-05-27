using System.ComponentModel.DataAnnotations;

namespace Counselor.Platform.Api.Entities.Dto
{
	public class AuthDto
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
