﻿using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Counselor.Platform.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Repositories
{
	public class DialogsRepository : IDialogsRepository
	{
		private readonly ConcurrentDictionary<int, Dialog> _dialogs = new ConcurrentDictionary<int, Dialog>();

		public async Task<Dialog> CreateOrUpdateDialogAsync(IPlatformDatabase dbContext, User client, string payload, MessageDirection direction, int botId = default)
		{
			var message = new Message
			{
				Payload = payload,
				Direction = direction
			};

			if (!_dialogs.TryGetValue(client.Id, out var dialog))
			{
				if (botId < 1)
					throw new ArgumentOutOfRangeException(nameof(botId), "Dialog cannot be created without botId.");

				dialog = new Dialog
				{
					Client = client,
					State = DialogState.Active,
					BotId = botId,
					Messages = new List<Message>
					{
						message
					}
				};

				_dialogs.TryAdd(client.Id, dialog);

				//todo: нужна логика поиска диалога, пока что в случае если текущий диалог не будет найден в репо, то будет создан новый
				await dbContext.Dialogs.AddAsync(dialog);
			}
			else
			{
				dialog.Messages.Add(message);
				dbContext.Dialogs.Update(dialog);
			}

			dialog.CurrentMessage = message;
			await dbContext.SaveChangesAsync();

			return dialog;
		}

		public async Task<Message> CreateDialogMessage(IPlatformDatabase dbContext, Dialog dialog, string payload, MessageDirection direction)
		{
			var message =
				new Message
				{
					Payload = payload,
					Direction = direction
				};

			dialog.Messages.Add(message);
			dialog.CurrentMessage = message;

			dbContext.Dialogs.Update(dialog);
			await dbContext.SaveChangesAsync();

			return message;
		}

		public Task FinishDialogAsync(IPlatformDatabase dbContext, Dialog dialog)
		{
			dialog.State = DialogState.Finished;
			dbContext.Dialogs.Update(dialog);
			return dbContext.SaveChangesAsync();
		}
	}
}
