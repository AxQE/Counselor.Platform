using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Exceptions;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services
{
	public class BotService : IBotService
	{
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

		public async Task<BotDto> Get(int botId, int userId)
			=> (await FindBot(botId, userId)).Adapt<BotDto>();

		public async Task<IEnumerable<BotDto>> GetAll(int userId)
		{
			var bots = await _database.Bots
				.AsNoTracking()
				.Where(x => x.Owner.Id == userId)
				.ToListAsync();

			return bots.Adapt<IEnumerable<BotDto>>();
		}

		public async Task<bool> Activate(int botId, int userId)
		{
			try
			{
				var bot = await FindBot(botId, userId);

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

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during bot activating.");
				throw;
			}
		}

		public async Task<BotDto> Create(BotDto bot)
		{
			try
			{
				var owner = await _database.Users.FirstOrDefaultAsync(x => x.Id == bot.Owner.Id);
				var script = await _database.Scripts.FirstOrDefaultAsync(x => x.Id == bot.Script.Id && x.Owner.Id == bot.Owner.Id);
				var transport = await _database.Transports.FirstOrDefaultAsync(x => x.Id == bot.Transport.Id);

				if (owner == null)
				{
					throw new GenericApiException((int)HttpStatusCode.UnprocessableEntity);
				}
				else if (script == null)
				{

				}
				else if (transport == null)
				{

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

				var result = await _database.Bots.AddAsync(dbBot);
				await _database.SaveChangesAsync();

				return result.Adapt<BotDto>();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during bot creation.");
				throw;
			}
		}

		public async Task<bool> Deactivate(int botId, int userId)
		{
			throw new NotImplementedException();
		}		

		public async Task<BotDto> Update(BotDto bot)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> Validate(int botId, int userId)
		{
			throw new NotImplementedException();
		}

		private Task<Bot> FindBot(int botId, int userId)
		{
			return (from dbBot in _database.Bots
						  where dbBot.Id == botId && dbBot.Owner.Id == userId
						  select dbBot).FirstOrDefaultAsync();
		}		
	}
}
