using Counselor.Platform.Api.Exceptions;
using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Factories;
using Counselor.Platform.Data.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Middleware
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
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

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			Envelope<object> errorResponse = null;
			HttpStatusCode statusCode = (HttpStatusCode)context.Response.StatusCode;

			if (statusCode == HttpStatusCode.OK)
			{
				statusCode = HttpStatusCode.InternalServerError;
			}

			if (exception != null)
			{
				if (exception is GenericApiException genericException)
				{
					errorResponse = EnvelopeFactory.Create<object>(statusCode, null, genericException.Message, genericException.ErrorId);
					_logger.LogError(exception, genericException.Message);
				}
				else if(exception is AccessDeniedException accessDeniedException)
				{
					statusCode = HttpStatusCode.Forbidden;
					errorResponse = EnvelopeFactory.Create<object>(statusCode, null, accessDeniedException.Message);
				}
				else
				{
					errorResponse = EnvelopeFactory.Create<object>(statusCode, null, "Something went wrong.");
					_logger.LogError(exception, "Unhandled API exception.");
				}
			}

			if (errorResponse != null)
			{
				context.Response.Clear();
				await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
			}

			context.Response.StatusCode = (int)statusCode;
			context.Response.ContentType = "application/json";
		}
	}
}
