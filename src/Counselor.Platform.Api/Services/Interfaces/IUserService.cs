using Counselor.Platform.Api.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IUserService
	{
		Task<UserDto> CreateUser(UserDto user);
		Task<UserDto> GetUser(int id);		
	}
}
