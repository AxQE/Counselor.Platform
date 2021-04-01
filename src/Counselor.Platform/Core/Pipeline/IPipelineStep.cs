using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline
{
	//todo: стоит подумать над медиатором
	public interface IPipelineStep
	{
		public int StepPriority { get; }		
		Task ExecuteAsync(IPlatformDatabase database, IOutgoingService outgoingService, Dialog dialog, string transport);
	}

	public class PipelineStepComparer : IComparer<IPipelineStep>
	{
		public int Compare(IPipelineStep x, IPipelineStep y)
		{
			if (x?.StepPriority > y?.StepPriority)
				return 1;
			else if (x?.StepPriority < y?.StepPriority)
				return -1;
			else
				return 0;
		}
	}
}
