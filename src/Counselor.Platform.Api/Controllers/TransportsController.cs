using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[Produces("application/json")]
	[ApiController]
	public class TransportsController : Controller
	{
		private readonly ITransportService _service;

		public TransportsController(ITransportService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransportDto))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllTransports(CancellationToken cancellationToken)
		{
			return Ok(await _service.GetAllTransports(cancellationToken));
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransportDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetTransportById(int id, CancellationToken cancellationToken)
		{
			if (id < 0) return BadRequest();

			var result = await _service.GetTransportById(id, cancellationToken);

			if (result == null) return NotFound();

			return Ok(result);
		}

		[HttpGet("{id}/commands")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransportDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetTransportCommands(int id, CancellationToken cancellationToken)
		{
			if (id < 0) return BadRequest();

			throw new NotImplementedException();
		}
	}
}
