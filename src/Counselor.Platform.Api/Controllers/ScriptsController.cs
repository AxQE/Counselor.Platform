using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Helpers;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Requests;

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
	public class ScriptsController : ControllerBase
	{
		private readonly IScriptService _service;

		public ScriptsController(IScriptService service)
		{
			_service = service;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Envelope<ScriptDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();

			return ResolveResponse(await _service.GetScript(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpGet]
		[ProducesResponseType(typeof(Envelope<IEnumerable<ScriptHeaderDto>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllScripts(CancellationToken cancellationToken)
		{
			return ResolveResponse(await _service.GetAllScripts(HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> CreateScript(ScriptCreateRequest script, CancellationToken cancellationToken)
		{
			var created = await _service.Create(script, HttpContext.GetCurrentUserId(), cancellationToken);
			return ResolveResponse(created);
		}

		[HttpPatch("{id}/activate")]
		[ProducesResponseType(typeof(Envelope<IEnumerable<ScriptHeaderDto>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> ActivateScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();
			return ResolveResponse(await _service.Activate(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPatch("{id}/deactivate")]
		[ProducesResponseType(typeof(Envelope<ScriptHeaderDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> DeactivateScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();
			return ResolveResponse(await _service.Deactivate(id, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPatch]
		[ProducesResponseType(typeof(Envelope<ScriptHeaderDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> UpdateScript(ScriptUpdateRequest script, CancellationToken cancellationToken)
		{
			return ResolveResponse(await _service.Update(script, HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> DeleteScript(int id, CancellationToken cancellationToken)
		{
			await _service.Delete(id, HttpContext.GetCurrentUserId(), cancellationToken);
			return NoContent();
		}
	}
}
