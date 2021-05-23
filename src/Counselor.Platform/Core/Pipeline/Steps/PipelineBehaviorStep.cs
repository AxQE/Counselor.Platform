using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Repositories;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Pipeline.Steps
{
	class PipelineBehaviorStep : IPipelineStep
	{
		private readonly BehaviorRepository _behaviorManager;
		private readonly IBehaviorExecutor _executor;
		private readonly ILogger<PipelineBehaviorStep> _logger;

		public PipelineBehaviorStep(
			BehaviorRepository behaviorRepository, 
			IBehaviorExecutor executor,
			ILogger<PipelineBehaviorStep> logger)
		{
			_behaviorManager = behaviorRepository;
			_executor = executor;
			_logger = logger;
		}

		public int StepPriority => 50;

		public async Task ExecuteAsync(IPlatformDatabase database, IOutgoingService outgoingService, Dialog dialog, string transport)
		{
			if (string.IsNullOrEmpty(dialog.Name))
				throw new ArgumentNullException(nameof(dialog.Name));

			var behaviorIterator = _behaviorManager.GetBehavior(dialog.Name);

			while (behaviorIterator.Current() != null)
			{
				foreach (var step in behaviorIterator.Current())
				{
					if (step.IsActive)
					{						
						await _executor.RunStep(database, outgoingService, dialog, step);
					}
				}

				behaviorIterator.Next();
			}
		}
	}
}
