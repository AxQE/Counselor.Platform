using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Expressions;
using Counselor.Platform.Interpreter.Templates;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter
{
	public class InterpreterRuntime : IInterpreter
	{
		private readonly ILogger<InterpreterRuntime> _logger;
		private readonly IExpressionParser _expressionParser;

		public InterpreterRuntime(
			ILogger<InterpreterRuntime> logger,
			IExpressionParser expressionParser
			)
		{
			_logger = logger;
			_expressionParser = expressionParser;
		}

		public async Task<InterpretationResult> Interpret(IInstruction instruction, Dialog dialog, IPlatformDatabase database, string transport)
		{
			try
			{
				var enrichedInstruction = await InsertEntityParameters(instruction.Instruction, dialog, database);
				var expression = _expressionParser.Parse(enrichedInstruction, transport);
				return await expression.InterpretAsync(database, dialog);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Interpretation failed. Instruction: {instruction.Instruction}.");

				return new InterpretationResult
				{
					Result = ex,
					State = InterpretationResultState.Failed
				};
			}
		}

		public async Task<string> InsertEntityParameters(string message, Dialog dialog, IPlatformDatabase database)
		{
			return await TextTemplateHandler.InsertEntityParameters(message, dialog, database);
		}

		public void Dispose()
		{
			//todo: dispose
		}

		public Task<InterpretationResult> Interpret(InterpretationContext context)
		{
			throw new NotImplementedException();
		}
	}
}
