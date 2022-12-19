using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Models.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IUserService
	{
		Task<Envelope<UserDto>> Authenticate(string username, string password, CancellationToken cancellationToken);
		Task<Envelope<UserDto>> CreateUser(UserCreateRequest data, CancellationToken cancellationToken);
		Task<Envelope<UserDto>> GetCurrentUser(int userId, CancellationToken cancellationToken);
	}
}
