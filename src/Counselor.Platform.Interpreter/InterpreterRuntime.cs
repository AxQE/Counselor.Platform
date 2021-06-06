using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Expressions;
using Counselor.Platform.Interpreter.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter
{
	public class InterpreterRuntime : IInterpreter
	{
		private static readonly TextTemplateHandler _templateHandler = new TextTemplateHandler();

		public async Task<InterpretationResult> Interpret(IInstruction instruction, Dialog dialog, IPlatformDatabase database)
		{
			var expression = ExpressionParser.Parse(instruction);

			return await expression.Interpret(database, dialog);
		}

		public Task<bool> InterpretLogical(IInstruction instruction, Dialog dialog, IPlatformDatabase database)
		{
			throw new NotImplementedException();
		}

		public async Task<string> InsertEntityParameters(string message, Dialog dialog, IPlatformDatabase database)
		{
			return await _templateHandler.InsertEntityParameters(message, dialog, database);
		}

		public void Dispose()
		{
			//todo: dispose
		}		
	}
}
