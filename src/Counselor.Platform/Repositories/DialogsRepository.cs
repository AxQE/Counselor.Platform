using Counselor.Platform.Core;
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
		private readonly IPlatformDatabase _database;
		private readonly ConcurrentDictionary<int, IDialog> _dialogs = new ConcurrentDictionary<int, IDialog>();

		public DialogsRepository(IPlatformDatabase database)
		{
			_database = database;
		}

		public async Task<IDialog> GetDialogAsync(User user, string payload)
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
				await _database.Dialogs.AddAsync(dialog as Dialog);				
			}
			else
			{
				dialog.Messages.Add(
					new Message
					{
						Id = new Guid(),
						Payload = payload
					});

				_database.Dialogs.Update(dialog as Dialog);
			}

			await _database.SaveChangesAsync();

			return dialog;
		}
	}
}
