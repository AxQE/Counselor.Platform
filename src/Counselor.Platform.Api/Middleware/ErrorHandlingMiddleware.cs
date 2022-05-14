using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Middleware
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
				throw;
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			return Task.CompletedTask;
		}
	}
}
