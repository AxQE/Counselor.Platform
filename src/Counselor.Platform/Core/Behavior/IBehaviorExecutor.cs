using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	interface IBehaviorExecutor : IDisposable
	{
		Task RunBehaviorLogicAsync(string connectionId, string username, string payload, string transport, string dialog);
	}
}
