using Counselor.Platform.Api.Entities.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IScriptService
	{
		Task<ScriptDto> GetScript(int id, int userId, CancellationToken cancellationToken);
		Task<IEnumerable<ScriptHeaderDto>> GetAllScripts(int userId, CancellationToken cancellationToken);
		Task<ScriptHeaderDto> Create(ScriptDto script, int userId, CancellationToken cancellationToken);
		Task<ScriptHeaderDto> Update(ScriptDto script, int userId, CancellationToken cancellationToken);
		Task Delete(int id, int userId, CancellationToken cancellationToken);
		Task<IEnumerable<ScriptHeaderDto>> Activate(int scriptId, CancellationToken cancellationToken);
		Task<ScriptHeaderDto> Deactivate(int scriptId, CancellationToken cancellationToken);
	}
}
