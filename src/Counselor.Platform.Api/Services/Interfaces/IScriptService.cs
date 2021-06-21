using Counselor.Platform.Api.Entities.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IScriptService
	{
		Task<ScriptDto> GetScript(int id);
		Task<IEnumerable<ScriptHeaderDto>> GetAllScripts();
		Task<ScriptHeaderDto> Create(ScriptDto script);
		Task<ScriptHeaderDto> Update(ScriptDto script);
		Task Delete(int id);
		Task<IEnumerable<ScriptHeaderDto>> Activate(ScriptHeaderDto script);
		Task<ScriptHeaderDto> Deactivate(ScriptHeaderDto script);
	}
}
