using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline
{
	internal interface IPipelineExecutor : IDisposable
	{
		Task<PipelineResult> RunAsync(string connectionId, string username, string payload, string transport, string dialog);
	}
}
