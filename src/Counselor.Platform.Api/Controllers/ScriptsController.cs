using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
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
		public async Task<IActionResult> GetScript(int id)
		{
			if (id <= 0) return BadRequest();

			var script = await _service.GetScript(id);

			if (script == null) return NotFound();

			return Ok(script);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ScriptHeaderDto>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllScripts()
		{
			return Ok(await _service.GetAllScripts());
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> CreateScript(ScriptDto script)
		{
			var created = await _service.Create(script);
			return Created(string.Empty, created);
		}

		[HttpPatch("activate")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ScriptHeaderDto>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ActivateScript(ScriptHeaderDto script)
		{
			if (script.Id <= 0) return BadRequest();
			return Ok(await _service.Activate(script));
		}

		[HttpPatch("deactivate")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ScriptHeaderDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeactivateScript(ScriptHeaderDto script)
		{
			if (script.Id <= 0) return BadRequest();
			return Ok(await _service.Deactivate(script));			
		}

		[HttpPatch]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateScript(ScriptDto behavior)
		{
			throw new NotImplementedException();
		}

		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeleteScript(int id)
		{
			await _service.Delete(id);
			return NoContent();
		}
	}
}
