using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Helpers
{
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly IUserService _service;

		public BasicAuthenticationHandler(
			IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IUserService service)
			: base(options, logger, encoder, clock)
		{
			_service = service;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var endpoint = Context.GetEndpoint();
			if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
				return AuthenticateResult.NoResult();

			if (!Request.Headers.ContainsKey("Authorization"))
				return AuthenticateResult.Fail("Missing Authorization Header");

			Envelope<UserDto> authResult = null;
			try
			{
				var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
				var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
				var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
				var username = credentials[0];
				var password = credentials[1];
				authResult = await _service.Authenticate(username, password, CancellationToken.None);
			}
			catch
			{
				return AuthenticateResult.Fail("Invalid Authorization Header");
			}

			if (authResult.Payload == null)
				return AuthenticateResult.Fail(authResult.Error.Message);

			var claims = new[] {
				new Claim(ClaimTypes.NameIdentifier, authResult.Payload.Id.ToString()),
				new Claim(ClaimTypes.Name, authResult.Payload.Username),
			};
			var identity = new ClaimsIdentity(claims, Scheme.Name);
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, Scheme.Name);

			return AuthenticateResult.Success(ticket);
		}
	}
}
