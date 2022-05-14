﻿using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface IScriptService
	{
		Task<Envelope<ScriptDto>> GetScript(int id, int userId, CancellationToken cancellationToken);
		Task<Envelope<IEnumerable<ScriptHeaderDto>>> GetAllScripts(int userId, CancellationToken cancellationToken);
		Task<Envelope<ScriptHeaderDto>> Create(ScriptDto script, int userId, CancellationToken cancellationToken);
		Task<Envelope<ScriptHeaderDto>> Update(ScriptDto script, int userId, CancellationToken cancellationToken);
		Task Delete(int id, int userId, CancellationToken cancellationToken);
		Task<Envelope<IEnumerable<ScriptHeaderDto>>> Activate(int scriptId, int userId, CancellationToken cancellationToken);
		Task<Envelope<ScriptHeaderDto>> Deactivate(int scriptId, int userId, CancellationToken cancellationToken);
	}
}
