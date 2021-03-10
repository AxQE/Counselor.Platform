using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline
{
	internal interface IPipelineExecutor
	{
		Task<PipelineResult> RunAsync(string connectionId, string username, string payload, string transport);
	}
}
