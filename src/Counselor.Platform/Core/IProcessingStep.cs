using System.Threading.Tasks;

namespace Counselor.Platform.Core
{
	public interface IProcessingStep
	{
		public int StepPriority { get; }
		Task Execute(IDialog dialog);
	}
}
