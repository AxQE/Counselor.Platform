using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Microsoft.Extensions.Logging;

namespace Counselor.Platform.Api.Services
{
	public class DialogService : IDialogService
	{
		public DialogService(
			IPlatformDatabase database,
			ILogger<DialogService> logger)
		{

		}
	}
}
