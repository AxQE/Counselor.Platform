﻿using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
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
		public async Task<IActionResult> Authenticate(AuthDto auth, CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid) return BadRequest();

			var user = await _service.Authenticate(auth, cancellationToken);

			return ResolveResponse(user);
		}

		[AllowAnonymous]
		[HttpPost]
		[ProducesResponseType(typeof(Envelope<UserDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> CreateUser(AuthDto auth, CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid) return BadRequest();

			var newUser = await _service.CreateUser(auth, cancellationToken);

			return ResolveResponse(newUser);
		}

		[HttpGet("current")]
		[ProducesResponseType(typeof(Envelope<UserDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
		{
			var user = await _service.GetCurrentUser(HttpContext.User, cancellationToken);

			return ResolveResponse(user);
		}
	}
}
