using Counselor.Platform.Api.Entities.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		[HttpPost]
		public async Task<ActionResult<UserDto>> CreateUser(UserDto user)
		{
			throw new NotImplementedException();
		}

		[HttpGet("{id}")]		
		public async Task<ActionResult<UserDto>> GetUser(int id)
		{
			throw new NotImplementedException();
		}
	}
}
