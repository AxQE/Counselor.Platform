using System.Threading.Tasks;

namespace Counselor.Platform.Core
{
	public interface IPipelineExecutor
	{
		Task<PipelineResult> RunAsync(IDialog dialog);
	}
}
