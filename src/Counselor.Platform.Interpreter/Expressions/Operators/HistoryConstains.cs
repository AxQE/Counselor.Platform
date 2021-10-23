using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Commands;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions.Operators
{
	[InterpreterCommand(null, typeof(string))]
	class HistoryConstains : IExpression
	{
		private readonly string _parameters;
		public string Operator => nameof(HistoryConstains);

		public HistoryConstains(string parameters)
		{
			_parameters = parameters;
		}

		public Task<InterpretationResult> InterpretAsync(IPlatformDatabase database, Dialog dialog)
		{
			throw new NotImplementedException();
		}
	}
}
