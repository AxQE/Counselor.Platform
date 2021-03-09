using Counselor.Platform.Entities;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline
{
	public interface IPipelineExecutor
	{
		Task<PipelineResult> RunAsync(string connectionId, string username, string payload, string transport);
	}
}
