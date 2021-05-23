using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	interface IBehaviorExecutor
	{
		Task RunStep(IPlatformDatabase database, IOutgoingService outgoingService, Dialog dialog, BehaviorStep behaviorStep);
	}
}
