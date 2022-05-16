using Counselor.Platform.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
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

				if (context.Response.StatusCode >= 400 && !context.Response.HasStarted)
				{
					await HandleExceptionAsync(context, null);
				}
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var errors = new List<string>();
			HttpStatusCode statusCode = (HttpStatusCode)context.Response.StatusCode;

			if (statusCode == HttpStatusCode.OK)
			{
				statusCode = HttpStatusCode.InternalServerError;
			}

			if (exception != null)
			{
				if (exception is GenericApiException genericException)
				{
				}
				else
				{

				}
			}
		}
	}
}
