using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Database;
using Counselor.Platform.Entities;
using Counselor.Platform.Services;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline.Steps
{
	class PipelineBehaviorStep : IPipelineStep
	{
		private readonly IBehaviorManager _behaviorManager;

		public PipelineBehaviorStep(IBehaviorManager behaviorManager)
		{
			_behaviorManager = behaviorManager;
		}

		public int StepPriority => 50;

		public async Task ExecuteAsync(IPlatformDatabase database, IOutgoingService outgoingService, Dialog dialog, string transport)
		{
			if (string.IsNullOrEmpty(dialog.Name))
				throw new ArgumentNullException(nameof(dialog.Name));

			var behavior = _behaviorManager.GetBehavior(dialog.Name);

			while (behavior.Current() != null)
			{
				foreach (var step in behavior.Current())
				{
					
				}

				behavior.Next();
			}
		}
	}
}
