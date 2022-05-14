using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IBotService
	{
		Task<Envelope<BotDto>> Get(int botId, int userId, CancellationToken cancellationToken);
		Task<Envelope<IEnumerable<BotDto>>> GetAll(int userId, CancellationToken cancellationToken);
		Task<Envelope<BotDto>> Create(BotDto bot, int userId, CancellationToken cancellationToken);
		Task<Envelope<BotDto>> Update(BotDto bot, int userId, CancellationToken cancellationToken);
		Task<Envelope<OperationResultDto>> Activate(int botId, int userId, CancellationToken cancellationToken);
		Task<Envelope<OperationResultDto>> Deactivate(int botId, int userId, CancellationToken cancellationToken);
		Task<Envelope<OperationResultDto>> Validate(int botId, int userId, CancellationToken cancellationToken);
	}
}
