using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter;
using Counselor.Platform.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorExecutor : IBehaviorExecutor
	{
		private readonly IInterpreter _interpreter;
		private readonly ILogger<BehaviorExecutor> _logger;

		private static readonly Dictionary<string, Action> PredefinedBehaviorCommands = new Dictionary<string, Action>
		{
			{ "Next", null },
			{ "None", null },
			{ "Stop", null }
		};

		public BehaviorExecutor(IInterpreter interpreter, ILogger<BehaviorExecutor> logger)
		{
			_interpreter = interpreter;
			_logger = logger;
		}

		public async Task RunStep(IPlatformDatabase database, IOutgoingService outgoingService, Dialog dialog, BehaviorStep step)
		{
			try
			{
				//if (!(await _interpreter.Interpret(step.Condition, dialog, database)).GetTypedResult<bool>())
				//{
				//	return;
				//}

				if (step.Command != null)
				{
					if (!string.IsNullOrEmpty(step.Command?.Name) && PredefinedBehaviorCommands.TryGetValue(step.Command.Name, out var action))
					{
						action?.Invoke();
					}
					else
					{
						var result = await _interpreter.Interpret(step.Command, dialog, database);
					}
				}

				var response = await _interpreter.InsertEntityParameters(step.Response, dialog, database);

				await outgoingService.SendAsync(response, dialog.User.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Behavior step failed. Id: {step.Id}.");
				throw;
			}
		}
	}
}
