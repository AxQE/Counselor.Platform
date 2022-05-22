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
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class TransportsController : ControllerBase
	{
		private readonly ITransportService _service;

		public TransportsController(ITransportService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(typeof(Envelope<TransportDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllTransports([FromQuery] bool onlyActive, CancellationToken cancellationToken)
		{
			return ResolveResponse(await _service.GetAllTransports(onlyActive, cancellationToken));
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Envelope<TransportDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTransportById(int id, CancellationToken cancellationToken)
		{
			if (id < 0) return BadRequest();

			return ResolveResponse(await _service.GetTransportById(id, cancellationToken));
		}

		[HttpGet("{id}/commands")]
		[ProducesResponseType(typeof(Envelope<TransportDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTransportCommands(int id, CancellationToken cancellationToken)
		{
			if (id < 0) return BadRequest();

			return ResolveResponse(await _service.GetTranposportCommands(id, cancellationToken));
		}
	}
}
