using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Counselor.Platform.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[Produces("application/json")]
	[ApiController]
	public class DialogsController : ControllerBase
	{
		private readonly IDialogService _service;

		public DialogsController(IDialogService service)
		{
			_service = service;
		}
	}
}
