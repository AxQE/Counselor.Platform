using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Models.Factories;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services
{
	public class BotService : IBotService
	{
		private const string ScriptNotFoundOrNotOwned = "";

		private readonly ILogger<BotService> _logger;
		private readonly IPlatformDatabase _database;

		public BotService(
			ILogger<BotService> logger,
			IPlatformDatabase database
			)
		{
			_logger = logger;
			_database = database;
		}

		public async Task<Envelope<BotDto>> Get(int botId, int userId, CancellationToken cancellationToken)
			=> EnvelopeFactory.Create<BotDto>(HttpStatusCode.OK, await FindBot(botId, userId, cancellationToken));

		public async Task<Envelope<IEnumerable<BotDto>>> GetAll(int userId, CancellationToken cancellationToken)
		{
			var bots = await _database.Bots
				.AsNoTracking()
				.Where(x => x.Owner.Id == userId)
				.Include(x => x.Transport)
				.Include(x => x.Owner)
				.Include(x => x.Script)
				.ToListAsync();

			return EnvelopeFactory.Create<IEnumerable<BotDto>>(HttpStatusCode.OK, bots);
		}

		public async Task<Envelope<OperationResultDto>> Activate(int botId, int userId, CancellationToken cancellationToken)
		{
			try
			{
				var bot = await FindBot(botId, userId, cancellationToken, true);

				switch (bot.BotState)
				{
					case BotState.Created:
						break;
					case BotState.Invalid:
						break;
					case BotState.Pending:
						break;
					case BotState.Started:
						break;
					case BotState.Stopped:
						break;
					case BotState.Deactivated:
						break;
					default:
						break;
				}

				bot.BotState = BotState.Pending;

				_database.Bots.Update(bot);
				await _database.SaveChangesAsync();

				return EnvelopeFactory.Create<OperationResultDto>(HttpStatusCode.OK, new OperationResultDto { Result = OperationResult.Success });
			}
			catch (OperationCanceledException)
			{
				return EnvelopeFactory.Create<OperationResultDto>(HttpStatusCode.OK, new OperationResultDto { Result = OperationResult.Failed });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during bot activating.");
				throw;
			}
		}

		public async Task<Envelope<BotDto>> Create(BotDto bot, int userId, CancellationToken cancellationToken) //todo: странно что id владельца передается в теле, нужно брать из клеймов
		{
			try
			{
				var owner = await _database.Users.FirstOrDefaultAsync(x => x.Id == bot.Owner.Id, cancellationToken);
				var script = await _database.Scripts.FirstOrDefaultAsync(x => x.Id == bot.Script.Id && x.Owner.Id == bot.Owner.Id, cancellationToken);
				var transport = await _database.Transports.FirstOrDefaultAsync(x => x.Id == bot.Transport.Id, cancellationToken);

				if (owner == null) //todo: блок проверок закрыт заглушками
				{
					return EnvelopeFactory.Create<BotDto>(HttpStatusCode.UnprocessableEntity, errorMessage: ScriptNotFoundOrNotOwned);
				}
				else if (script == null)
				{
					return EnvelopeFactory.Create<BotDto>(HttpStatusCode.UnprocessableEntity, errorMessage: ScriptNotFoundOrNotOwned);
				}
				else if (transport == null)
				{
					return EnvelopeFactory.Create<BotDto>(HttpStatusCode.UnprocessableEntity, errorMessage: ScriptNotFoundOrNotOwned);
				}

				var dbBot = new Bot
				{
					Name = bot.Name,
					Owner = owner,
					Script = script,
					BotState = BotState.Created,
					Transport = transport,
					Configuration = bot.Configuration
				};

				var result = await _database.Bots.AddAsync(dbBot, cancellationToken);
				await _database.SaveChangesAsync(cancellationToken);

				return EnvelopeFactory.Create<BotDto>(HttpStatusCode.OK, result.Entity);
			}
			catch (OperationCanceledException)
			{
				return EnvelopeFactory.Create<BotDto>(HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during bot creation.");
				throw;
			}
		}

		public async Task<Envelope<OperationResultDto>> Deactivate(int botId, int userId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task<Envelope<BotDto>> Update(BotDto bot, int userId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task<Envelope<OperationResultDto>> Validate(int botId, int userId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		private Task<Bot> FindBot(int botId, int userId, CancellationToken cancellationToken, bool tracking = false)
		{
			var request = _database.Bots
				.Include(x => x.Owner)
				.Include(x => x.Transport)
				.Include(x => x.Script);

			if (tracking)
			{
				request.AsNoTracking();
			}

			return request.FirstOrDefaultAsync(x => x.Id == botId && x.Owner.Id == userId, cancellationToken);
		}
	}
}
