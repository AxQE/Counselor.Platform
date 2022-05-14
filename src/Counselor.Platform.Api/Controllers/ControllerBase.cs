using Counselor.Platform.Api.Helpers;
using Counselor.Platform.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Counselor.Platform.Api.Controllers
{
	public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		protected int CurrentUserId => HttpContext.GetCurrentUserId();

		protected IActionResult ResolveResponse<TData>(Envelope<TData> envelope) where TData : class
		{
			switch (envelope.HttpStatus)
			{
				case System.Net.HttpStatusCode.OK:
					return Ok(envelope);

				case System.Net.HttpStatusCode.Created:
					return Created(string.Empty, envelope);

				case System.Net.HttpStatusCode.Unauthorized:
					return Unauthorized();

				case System.Net.HttpStatusCode.Forbidden:
					return Forbid();

				case System.Net.HttpStatusCode.NotFound:
					return NotFound();

				case System.Net.HttpStatusCode.UnprocessableEntity:
					return UnprocessableEntity(envelope);

				default: return Ok();
			}
		}
	}
}
