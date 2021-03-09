using Counselor.Platform.Entities;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline
{
	public interface IPipelineStep
	{
		public int StepPriority { get; }
		Task ExecuteAsync(Dialog dialog);
	}
}
