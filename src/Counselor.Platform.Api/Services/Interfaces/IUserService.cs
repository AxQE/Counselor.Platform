using Counselor.Platform.Api.Entities.Dto;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IUserService
	{
		Task<UserDto> Authenticate(AuthDto auth, CancellationToken cancellationToken);
		Task<UserDto> Authenticate(string username, string password, CancellationToken cancellationToken);
		Task<UserDto> CreateUser(AuthDto auth, CancellationToken cancellationToken);
		Task<UserDto> GetCurrentUser(ClaimsPrincipal principal, CancellationToken cancellationToken);
	}
}
