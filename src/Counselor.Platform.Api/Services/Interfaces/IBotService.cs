using Counselor.Platform.Api.Entities.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IBotService
	{
		Task<BotDto> Get(int botId, int userId, CancellationToken cancellationToken);
		Task<IEnumerable<BotDto>> GetAll(int userId, CancellationToken cancellationToken);
		Task<BotDto> Create(BotDto bot, CancellationToken cancellationToken);
		Task<BotDto> Update(BotDto bot, CancellationToken cancellationToken);
		Task<bool> Activate(int botId, int userId, CancellationToken cancellationToken);
		Task<bool> Deactivate(int botId, int userId, CancellationToken cancellationToken);
		Task<bool> Validate(int botId, int userId, CancellationToken cancellationToken);
	}
}
