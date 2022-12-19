using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Models.Factories;
using Counselor.Platform.Api.Models.Requests;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services
{
	public class UserService : IUserService
	{
		private const string AuthenticationFailed = "Incorrect username or password.";
		private const string UsernameAlreadyExists = "Username already exists.";
		private const string EmailAlreadyInUse = "Email already in use.";

		private readonly IPlatformDatabase _database;
		private readonly ILogger<UserService> _logger;

		public UserService(
			IPlatformDatabase database,
			ILogger<UserService> logger
			)
		{
			_database = database;
			_logger = logger;
		}

		public async Task<Envelope<UserDto>> Authenticate(string username, string password, CancellationToken cancellationToken)
		{
			try
			{
				var user = await _database.Users
					.AsNoTracking()
					.FirstOrDefaultAsync(x => EF.Functions.ILike(x.Username, $"%{username}%"));

				if (user == null)
				{
					return EnvelopeFactory.Create<UserDto>(HttpStatusCode.Unauthorized, errorMessage: AuthenticationFailed);
				}

				byte[] salt = Convert.FromBase64String(user.Salt);
				var hashedPassword = HashPassword(password, salt);

				if (!user.Password.Equals(hashedPassword))
				{
					return EnvelopeFactory.Create<UserDto>(HttpStatusCode.Unauthorized, errorMessage: AuthenticationFailed);
				}

				return EnvelopeFactory.Create<UserDto>(HttpStatusCode.OK, user);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during user authentication. Username: {Username}.", username);
				throw;
			}
		}

		public async Task<Envelope<UserDto>> CreateUser(UserCreateRequest data, CancellationToken cancellationToken)
		{
			var existingUsername = await _database.Users
						.AsNoTracking()
						.FirstOrDefaultAsync(x => EF.Functions.ILike(x.Username, $"%{data.Username}%"));

			if (existingUsername != null)
			{
				return EnvelopeFactory.Create<UserDto>(HttpStatusCode.UnprocessableEntity, null, UsernameAlreadyExists);
			}

			var existingEmail = await _database.Users
					.AsNoTracking()
					.FirstOrDefaultAsync(x => x.Email == data.Email.ToLowerInvariant());

			if (existingEmail != null)
			{
				return EnvelopeFactory.Create<UserDto>(HttpStatusCode.UnprocessableEntity, null, EmailAlreadyInUse);
			}

			byte[] salt = new byte[16]; //128 bit
			using var random = RandomNumberGenerator.Create();
			random.GetBytes(salt);

			var newUser = new User
			{
				Username = data.Username,
				Email = data.Email.ToLowerInvariant(),
				Password = HashPassword(data.Password, salt),
				Salt = Convert.ToBase64String(salt),
				//Role = Roles.Client
			};

			await _database.Users.AddAsync(newUser);
			await _database.SaveChangesAsync();

			return EnvelopeFactory.Create<UserDto>(HttpStatusCode.Created, newUser);
		}

		public async Task<Envelope<UserDto>> GetCurrentUser(int userId, CancellationToken cancellationToken)
		{
			var user = await _database.Users
						.AsNoTracking()
						.FirstOrDefaultAsync(x => x.Id == userId);

			if (user != null)
			{
				return EnvelopeFactory.Create<UserDto>(HttpStatusCode.OK, user);
			}

			return EnvelopeFactory.Create<UserDto>(HttpStatusCode.NotFound);
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
