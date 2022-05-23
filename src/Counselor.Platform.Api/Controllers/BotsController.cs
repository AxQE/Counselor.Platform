using Counselor.Platform.Api.Helpers;
using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Models.Requests;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class BotsController : ControllerBase
	{
		private readonly IBotService _botService;

		public BotsController(IBotService botService)
		{
			_botService = botService;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Envelope<BotDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
		{
			if (id < 1) return BadRequest();

			return ResolveResponse(await _botService.Get(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpGet]
		[ProducesResponseType(typeof(Envelope<IEnumerable<BotDto>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			return ResolveResponse(await _botService.GetAll(HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Create(BotCreateRequest bot, CancellationToken cancellationToken)
		{
			return ResolveResponse(await _botService.Create(bot, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPatch("{id}/activate")]
		[ProducesResponseType(typeof(Envelope<OperationResultDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken)
		{
			return ResolveResponse(await _botService.Activate(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPatch("{id}/deactivate")]
		[ProducesResponseType(typeof(Envelope<OperationResultDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
		{
			return ResolveResponse(await _botService.Deactivate(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}
	}
}
