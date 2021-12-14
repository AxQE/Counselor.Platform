using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Helpers;
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
	public class BotsController : ControllerBase
	{
		private readonly IBotService _botService;

		public BotsController(IBotService botService)
		{
			_botService = botService;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BotDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
		{
			if (id < 1) return BadRequest();

			BotDto result = await _botService.Get(id, HttpContext.GetCurrentUserId(), cancellationToken);

			if (result == null) return NotFound();

			return Ok(result);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BotDto>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			return Ok(await _botService.GetAll(HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create(BotDto bot, CancellationToken cancellationToken)
		{
			bot.Owner = HttpContext.GetCurrentUser();
			return Created(string.Empty, await _botService.Create(bot, cancellationToken));
		}

		[HttpPatch("{id}/activate")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken)
		{
			return Ok(await _botService.Activate(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPatch("{id}/deactivate")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
		{
			return Ok(await _botService.Deactivate(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}
	}
}
