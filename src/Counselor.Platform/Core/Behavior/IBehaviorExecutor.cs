using Counselor.Platform.Data.Database;
using Counselor.Platform.Services;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	public interface IBehaviorExecutor : IDisposable
	{
		void Initialize(int scriptId, IPlatformDatabase database);
		Task RunBehaviorLogicAsync(string connectionId, string username, string payload, ServiceContext context);
	}
}
