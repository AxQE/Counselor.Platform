using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Core
{
	public class PipelineExecutor : IPipelineExecutor
	{
		private readonly SortedSet<IProcessingStep> _processingSteps = new SortedSet<IProcessingStep>();
		private readonly ILogger<PipelineExecutor> _logger;

		public PipelineExecutor(ILogger<PipelineExecutor> logger)
		{
			_logger = logger;
		}

		public async Task<PipelineResult> Run(IDialog dialog)
		{
			var result = new PipelineResult();			

			try
			{
				foreach (var step in _processingSteps)
				{
					await step.Execute(dialog);
				}				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Message pipeline failed. DialogId: {dialog.Id}. UserId: {dialog.UserId}.");
				result.SuccessfullyCompleted = false;
			}

			return result;
		}
	}
}
