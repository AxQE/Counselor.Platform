using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Entities.Enums;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Repositories
{
	public class DialogsRepository
	{
		private readonly ConcurrentDictionary<int, Dialog> _dialogs = new ConcurrentDictionary<int, Dialog>();

		public DialogsRepository()
		{
		}

		public async Task<Dialog> CreateOrUpdateDialogAsync(IPlatformDatabase dbContext, User user, string payload, MessageDirection direction)
		{
			var message = new Message
			{
				Payload = payload,
				Direction = direction
			};

			if (!_dialogs.TryGetValue(user.Id, out var dialog))
			{				
				dialog = new Dialog
				{
					User = user,
					State = DialogState.Active,
					Messages = new List<Message>
					{
						message
					}
				};

				_dialogs.TryAdd(user.Id, dialog);

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

		public async Task FinishDialogAsync(IPlatformDatabase dbContext, Dialog dialog)
		{
			dialog.State = DialogState.Finished;
			dbContext.Dialogs.Update(dialog);
			await dbContext.SaveChangesAsync();
		}
	}
}
