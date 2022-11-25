using Counselor.Platform.Api.Helpers;
using Counselor.Platform.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Counselor.Platform.Api.Controllers
{
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		protected int CurrentUserId => HttpContext.GetCurrentUserId();

		protected IActionResult ResolveResponse<TData>(Envelope<TData> envelope) where TData : class
		{
			return StatusCode((int)envelope.HttpStatus, envelope);
		}
	}
}
