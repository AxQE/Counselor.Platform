using Counselor.Platform.Api.Entities.Dto;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IUserService
	{
		Task<UserDto> Authenticate(AuthDto auth);
		Task<UserDto> Authenticate(string username, string password);
		Task<UserDto> CreateUser(AuthDto auth);
		Task<UserDto> GetCurrentUser(ClaimsPrincipal principal);
	}
}
