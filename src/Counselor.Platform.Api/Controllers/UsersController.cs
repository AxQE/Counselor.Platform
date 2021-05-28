using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _service;

		public UsersController(IUserService service)
		{
			_service = service;
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]		
		public async Task<ActionResult<UserDto>> Authenticate(AuthDto auth)
		{
			if (!ModelState.IsValid) return BadRequest();

			var user = await _service.Authenticate(auth);

			if (user == null) return Unauthorized();

			return user;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult> CreateUser(AuthDto auth)
		{
			if (!ModelState.IsValid) return UnprocessableEntity();

			var newUser = await _service.CreateUser(auth);

			if (newUser == null) return UnprocessableEntity();

			return Created();
		}

		[HttpGet("current")]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var user = await _service.GetCurrentUser(HttpContext.User);

			if (user == null) return NotFound();
			
			return user;
		}
	}
}
