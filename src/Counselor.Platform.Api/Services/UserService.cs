using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

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
			throw new NotImplementedException();
		}

		public async Task<UserDto> GetUser(int id)
		{
			throw new NotImplementedException();
		}
	}
}
