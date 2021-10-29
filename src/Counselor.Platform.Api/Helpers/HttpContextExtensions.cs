using Counselor.Platform.Api.Entities.Dto;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Counselor.Platform.Api.Helpers
{
	public static class HttpContextExtensions
	{
		public static UserDto GetCurrentUser(this HttpContext httpContext)
		{
			return new UserDto
			{
				Id = int.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
				Username = httpContext.User.FindFirst(ClaimTypes.Name).Value
			};			
		}

		public static int GetCurrentUserId(this HttpContext httpContext)
		{
			return int.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
		}
	}
}
