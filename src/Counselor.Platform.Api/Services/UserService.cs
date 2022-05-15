using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Models.Factories;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services
{
	public class UserService : IUserService
	{
		private const string UserOrPasswordNotFound = "Incorrect username or password.";
		private const string UsernameAlreadyExists = "Username already axists.";

		private readonly IPlatformDatabase _database;
		private readonly ILogger<UserService> _logger;

		public UserService(IPlatformDatabase database, ILogger<UserService> logger)
		{
			_database = database;
			_logger = logger;
		}

		public Task<Envelope<UserDto>> Authenticate(AuthDto auth, CancellationToken cancellationToken)
		{
			return Authenticate(auth.Username, auth.Password, cancellationToken);
		}

		public async Task<Envelope<UserDto>> Authenticate(string username, string password, CancellationToken cancellationToken)
		{
			try
			{
				var user = await _database.Users
					.AsNoTracking()
					.FirstOrDefaultAsync(x => string.Equals(x.Username, username));

				if (user == null)
				{
					return EnvelopeFactory.Create<UserDto>(HttpStatusCode.Unauthorized, message: UserOrPasswordNotFound);
				}

				byte[] salt = Convert.FromBase64String(user.Salt);
				var hashedPassword = HashPassword(password, salt);

				if (!user.Password.Equals(hashedPassword))
				{
					return EnvelopeFactory.Create<UserDto>(HttpStatusCode.Unauthorized, message: UserOrPasswordNotFound);
				}

				return EnvelopeFactory.Create<UserDto>(HttpStatusCode.OK, user);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error during user authentication. Username: {username}.");
				throw;
			}
		}

		public async Task<Envelope<UserDto>> CreateUser(AuthDto auth, CancellationToken cancellationToken)
		{
			try
			{
				var exists = await _database.Users
						.AsNoTracking()
						.FirstOrDefaultAsync(x => string.Equals(x.Username, auth.Username));

				if (exists != null)
				{
					return EnvelopeFactory.Create<UserDto>(HttpStatusCode.UnprocessableEntity, null, UsernameAlreadyExists);
				}

				byte[] salt = new byte[16]; //128 bit
				using var random = RandomNumberGenerator.Create();
				random.GetBytes(salt);

				var newUser = new User
				{
					Username = auth.Username,
					Password = HashPassword(auth.Password, salt),
					Salt = Convert.ToBase64String(salt)
				};

				await _database.Users.AddAsync(newUser);
				await _database.SaveChangesAsync();

				return EnvelopeFactory.Create<UserDto>(HttpStatusCode.Created, newUser);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during user creation.");
				throw;
			}
		}

		public async Task<Envelope<UserDto>> GetCurrentUser(ClaimsPrincipal principal, CancellationToken cancellationToken)
		{
			try
			{
				int userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));

				var user = await _database.Users
						.AsNoTracking()
						.FirstOrDefaultAsync(x => x.Id == userId);

				if (user != null)
				{
					return EnvelopeFactory.Create<UserDto>(HttpStatusCode.OK, user);
				}

				return EnvelopeFactory.Create<UserDto>(HttpStatusCode.NotFound);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during getuser.");
				throw;
			}
		}

		private string HashPassword(string password, byte[] salt)
		{
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(
					   password: password,
					   salt: salt,
					   prf: KeyDerivationPrf.HMACSHA1,
					   iterationCount: 1000,
					   numBytesRequested: 32 //256 bit
					   ));
		}
	}
}
