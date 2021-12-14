using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[Produces("application/json")]
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
		public async Task<IActionResult> Authenticate(AuthDto auth, CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid) return BadRequest();

			var user = await _service.Authenticate(auth, cancellationToken);

			if (user == null) return Unauthorized();

			return Ok(user);
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> CreateUser(AuthDto auth, CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid) return UnprocessableEntity();

			var newUser = await _service.CreateUser(auth, cancellationToken);

			if (newUser == null) return UnprocessableEntity();

			return Created(string.Empty, newUser);
		}

		[HttpGet("current")]
		public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
		{
			var user = await _service.GetCurrentUser(HttpContext.User, cancellationToken);

			if (user == null) return NotFound();

			return Ok(user);
		}
	}
}
