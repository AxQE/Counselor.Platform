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
	public class ScriptsController : ControllerBase
	{
		private readonly IScriptService _service;

		public ScriptsController(IScriptService service)
		{
			_service = service;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ScriptDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();

			var script = await _service.GetScript(id, HttpContext.GetCurrentUserId(), cancellationToken);

			if (script == null) return NotFound();

			return Ok(script);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ScriptHeaderDto>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllScripts(CancellationToken cancellationToken)
		{
			return Ok(await _service.GetAllScripts(HttpContext.GetCurrentUserId(), cancellationToken));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> CreateScript(ScriptDto script, CancellationToken cancellationToken)
		{
			var created = await _service.Create(script, HttpContext.GetCurrentUserId(), cancellationToken);
			return Created(string.Empty, created);
		}

		[HttpPatch("{id}/activate")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ScriptHeaderDto>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ActivateScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();
			return Ok(await _service.Activate(id, cancellationToken));
		}

		[HttpPatch("{id}/deactivate")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ScriptHeaderDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeactivateScript(int id, CancellationToken cancellationToken)
		{
			if (id <= 0) return BadRequest();
			return Ok(await _service.Deactivate(id, cancellationToken));
		}

		[HttpPatch]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateScript(ScriptDto script, CancellationToken cancellationToken)
		{
			return Ok(await _service.Update(script, HttpContext.GetCurrentUserId(), cancellationToken));
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
