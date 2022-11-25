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
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Authenticate(AuthRequest auth, CancellationToken cancellationToken)
		{
			return ResolveResponse(await _service.Authenticate(auth, cancellationToken));
		}

		[AllowAnonymous]
		[HttpPost]
		[ProducesResponseType(typeof(Envelope<UserDto>), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> CreateUser(UserCreateRequest data, CancellationToken cancellationToken)
		{
			return ResolveResponse(await _service.CreateUser(data, cancellationToken));
		}

		[HttpGet("current")]
		[ProducesResponseType(typeof(Envelope<UserDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
		{
			return ResolveResponse(await _service.GetCurrentUser(CurrentUserId, cancellationToken));
		}
	}
}
