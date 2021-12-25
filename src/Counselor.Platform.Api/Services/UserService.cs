using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services
{
	public class UserService : IUserService
	{
		private readonly IPlatformDatabase _database;
		private readonly ILogger<UserService> _logger;

		public UserService(IPlatformDatabase database, ILogger<UserService> logger)
		{
			_database = database;
			_logger = logger;
		}

		public Task<UserDto> Authenticate(AuthDto auth, CancellationToken cancellationToken)
		{
			return Authenticate(auth.Username, auth.Password, cancellationToken);
		}

		public async Task<UserDto> Authenticate(string username, string password, CancellationToken cancellationToken)
		{
			try
			{
				var user = await _database.Users
					.AsNoTracking()
					.FirstOrDefaultAsync(x => string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase));

				byte[] salt = Convert.FromBase64String(user.Salt);
				var hashedPassword = HashPassword(password, salt);

				if (user == null || !user.Password.Equals(hashedPassword)) return null;

				return new UserDto
				{
					Id = user.Id,
					Username = user.Username
				};

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error during user authentication. Username: {username}.");
				throw;
			}
		}

		public async Task<UserDto> CreateUser(AuthDto auth, CancellationToken cancellationToken)
		{
			try
			{
				var exists = await _database.Users
						.AsNoTracking()
						.FirstOrDefaultAsync(x => string.Equals(x.Username, auth.Username, StringComparison.OrdinalIgnoreCase));

				if (exists != null) return null;

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

				return new UserDto
				{
					Id = newUser.Id,
					Username = newUser.Username
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during user creation.");
				throw;
			}
		}

		public async Task<UserDto> GetCurrentUser(ClaimsPrincipal principal, CancellationToken cancellationToken)
		{
			try
			{
				int userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));

				var user = await _database.Users
						.AsNoTracking()
						.FirstOrDefaultAsync(x => x.Id == userId);

				if (user != null)
				{
					return new UserDto
					{
						Id = user.Id,
						Username = user.Username
					};
				}

				return null;
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
