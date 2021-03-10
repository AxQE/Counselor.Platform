using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Services;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline
{
	public interface IPipelineStep
	{
		public int StepPriority { get; }
		Task ExecuteAsync(IPlatformDatabase database, IOutgoingService outgoingService, Dialog dialog, string transport);
	}
}
