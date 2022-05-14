using Counselor.Platform.Api.Models.Dto;
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
	public class ScriptsController : ControllerBase
	{
		private readonly IScriptService _service;

		public ScriptsController(IScriptService service)
		{
			_service = service;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ScriptDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();

			return ResolveResponse(await _service.GetScript(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<ScriptHeaderDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllScripts(CancellationToken cancellationToken)
		{
			return ResolveResponse(await _service.GetAllScripts(HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> CreateScript(ScriptDto script, CancellationToken cancellationToken)
		{
			var created = await _service.Create(script, HttpContext.GetCurrentUserId(), cancellationToken);
			return ResolveResponse(created);
		}

		[HttpPatch("{id}/activate")]
		[ProducesResponseType(typeof(IEnumerable<ScriptHeaderDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ActivateScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();
			return ResolveResponse(await _service.Activate(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPatch("{id}/deactivate")]
		[ProducesResponseType(typeof(ScriptHeaderDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeactivateScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();
			return ResolveResponse(await _service.Deactivate(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPatch]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateScript(ScriptDto script, CancellationToken cancellationToken)
		{
			return ResolveResponse(await _service.Update(script, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeleteScript(int id, CancellationToken cancellationToken)
		{
			await _service.Delete(id, HttpContext.GetCurrentUserId(), cancellationToken);
			return NoContent();
		}
	}
}
