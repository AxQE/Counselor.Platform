using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Models.Factories;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services
{
	public class ScriptService : IScriptService
	{
		private readonly IPlatformDatabase _database;
		private readonly ILogger<ScriptService> _logger;

		public ScriptService(
			IPlatformDatabase database,
			ILogger<ScriptService> logger
			)
		{
			_database = database;
			_logger = logger;
		}

		public async Task Delete(int id, int userId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task<Envelope<IEnumerable<ScriptHeaderDto>>> GetAllScripts(int userId, CancellationToken cancellationTokend)
		{
			try
			{
				var scripts = await _database.Scripts.ToListAsync(cancellationTokend);
				return EnvelopeFactory.Create<IEnumerable<ScriptHeaderDto>>(HttpStatusCode.OK, scripts);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Request failed.");
				throw;
			}
		}

		public async Task<Envelope<ScriptDto>> GetScript(int id, int userId, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _database.Scripts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
				return EnvelopeFactory.Create<ScriptDto>(HttpStatusCode.OK, result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Request failed. Unable to get script by id: {id}.");
				throw;
			}
		}

		public async Task<Envelope<ScriptHeaderDto>> Update(ScriptDto script, int userId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task<Envelope<ScriptHeaderDto>> Create(ScriptDto script, int userId, CancellationToken cancellationToken)
		{
			try
			{
				var owner = await _database.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
				Script dbScript = new Script
				{
					Name = script.Name,
					Instruction = script.Instruction,
					Owner = owner
				};

				var result = await _database.Scripts.AddAsync(dbScript, cancellationToken);
				await _database.SaveChangesAsync(cancellationToken);
				return EnvelopeFactory.Create<ScriptHeaderDto>(HttpStatusCode.Created, result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during script creation.");
				throw;
			}
		}

		public Task<Envelope<IEnumerable<ScriptHeaderDto>>> Activate(int scriptId, int userId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<Envelope<ScriptHeaderDto>> Deactivate(int scriptId, int userId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
