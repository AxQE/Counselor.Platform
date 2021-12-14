using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Counselor.Platform.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[Produces("application/json")]
	[ApiController]
	public class EntitiesController : ControllerBase
	{
		private readonly IEntitiesService _service;

		public EntitiesController(IEntitiesService service)
		{
			_service = service;
		}
	}
}
