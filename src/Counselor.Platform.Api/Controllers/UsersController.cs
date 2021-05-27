using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
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
		private readonly IUserService _service;

		public UsersController(IUserService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<ActionResult<UserDto>> CreateUser(UserDto user)
		{
			if (string.IsNullOrEmpty(user.Username))
				return UnprocessableEntity();

			var newUser = await _service.CreateUser(user);

			if (newUser == null) return UnprocessableEntity();

			return newUser;
		}

		[HttpGet("{id}")]		
		public async Task<ActionResult<UserDto>> GetUser(int id)
		{
			if (id <= 0) return BadRequest();

			var user = await _service.GetUser(id);

			if (user == null) return NotFound();
			
			return user;
		}
	}
}
