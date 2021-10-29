using Counselor.Platform.Api.Entities.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IBotService
	{
		Task<BotDto> Get(int botId, int userId);
		Task<IEnumerable<BotDto>> GetAll(int userId);
		Task<BotDto> Create(BotDto bot);
		Task<BotDto> Update(BotDto bot);
		Task<bool> Activate(int botId, int userId);
		Task<bool> Deactivate(int botId, int userId);
		Task<bool> Validate(int botId, int userId);
	}
}
