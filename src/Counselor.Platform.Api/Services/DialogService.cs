using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
