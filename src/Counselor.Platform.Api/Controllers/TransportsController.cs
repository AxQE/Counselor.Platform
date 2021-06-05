using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TransportsController : Controller
	{
		private readonly ITransportService _service;

		public TransportsController(ITransportService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTransports()
		{
			throw new NotImplementedException();
		}
	}
}
