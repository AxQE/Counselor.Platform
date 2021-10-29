using Counselor.Platform.Api.Entities.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IScriptService
	{
		Task<ScriptDto> GetScript(int id, int userId);
		Task<IEnumerable<ScriptHeaderDto>> GetAllScripts(int userId);
		Task<ScriptHeaderDto> Create(ScriptDto script, int userId);
		Task<ScriptHeaderDto> Update(ScriptDto script, int userId);
		Task Delete(int id, int userId);
		Task<IEnumerable<ScriptHeaderDto>> Activate(ScriptHeaderDto script);
		Task<ScriptHeaderDto> Deactivate(ScriptHeaderDto script);
	}
}
