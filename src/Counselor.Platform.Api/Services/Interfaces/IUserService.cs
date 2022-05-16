using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IUserService
	{
		Task<Envelope<UserDto>> Authenticate(AuthDto auth, CancellationToken cancellationToken);
		Task<Envelope<UserDto>> Authenticate(string username, string password, CancellationToken cancellationToken);
		Task<Envelope<UserDto>> CreateUser(AuthDto auth, CancellationToken cancellationToken);
		Task<Envelope<UserDto>> GetCurrentUser(int userId, CancellationToken cancellationToken);
	}
}
