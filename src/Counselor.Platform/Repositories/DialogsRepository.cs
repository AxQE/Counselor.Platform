using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using System;
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

		public async Task<Dialog> CreateOrUpdateDialogAsync(IPlatformDatabase dbContext, User user, string payload)
		{
			if (!_dialogs.TryGetValue(user.Id, out var dialog))
			{				
				dialog = new Dialog
				{
					Id = new Guid(),
					User = user,
					Messages = new List<Message>
					{
						new Message
						{
							Id = new Guid(),
							Payload = payload
						}
					}
				};

				_dialogs.TryAdd(user.Id, dialog);

				//todo: нужна логика поиска диалога, пока что в случае если текущий диалог не будет найден в репо, то будет создан новый
				await dbContext.Dialogs.AddAsync(dialog);
			}
			else
			{
				dialog.Messages.Add(
					new Message
					{
						Id = new Guid(),
						Payload = payload
					});

				dbContext.Dialogs.Update(dialog);
			}

			await dbContext.SaveChangesAsync();

			return dialog;
		}
	}
}
