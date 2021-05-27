using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Exceptions;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services
{
	public class UserService : IUserService
	{
		private readonly IPlatformDatabase _database;

		public UserService(IPlatformDatabase database)
		{
			_database = database;
		}

		public async Task<UserDto> CreateUser(UserDto user)
		{
			var exists = await _database.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Username.ToUpper() == user.Username.ToUpper());

			if (exists != null) return null;

			var newUser = new User
			{
				Username = user.Username
			};

			await _database.Users.AddAsync(newUser);

			await _database.SaveChangesAsync();

			return new UserDto
			{
				Id = newUser.Id,
				Username = newUser.Username
			};
		}

		public async Task<UserDto> GetUser(int id)
		{
			var user = await _database.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);

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
	}
}
