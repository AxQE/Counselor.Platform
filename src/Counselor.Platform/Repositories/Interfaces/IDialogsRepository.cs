using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using System.Threading.Tasks;

namespace Counselor.Platform.Repositories.Interfaces
{
	public interface IDialogsRepository
	{
		Task<Dialog> CreateOrUpdateDialogAsync(IPlatformDatabase dbContext, User user, string payload, MessageDirection direction, int botId);
		Task<Message> CreateDialogMessage(IPlatformDatabase dbContext, Dialog dialog, string payload, MessageDirection direction);
		Task FinishDialogAsync(IPlatformDatabase dbContext, Dialog dialog);
	}
}
