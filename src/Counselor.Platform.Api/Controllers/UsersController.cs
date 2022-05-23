using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Models.Requests;
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
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _service;

		public UsersController(IUserService service)
		{
			_service = service;
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]
		[ProducesResponseType(typeof(Envelope<UserDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> Authenticate(AuthRequest auth, CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid) return BadRequest();

			var user = await _service.Authenticate(auth, cancellationToken);

			return ResolveResponse(user);
		}

		[AllowAnonymous]
		[HttpPost]
		[ProducesResponseType(typeof(Envelope<UserDto>), StatusCodes.Status201Created)]
		public async Task<IActionResult> CreateUser(UserCreateRequest data, CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid) return BadRequest();

			var newUser = await _service.CreateUser(data, cancellationToken);

			return ResolveResponse(newUser);
		}

		[HttpGet("current")]
		[ProducesResponseType(typeof(Envelope<UserDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
		{
			var user = await _service.GetCurrentUser(CurrentUserId, cancellationToken);

			return ResolveResponse(user);
		}
	}
}
