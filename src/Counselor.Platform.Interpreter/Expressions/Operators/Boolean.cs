using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Commands;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions.Operators
{
	[InterpreterCommand(null, typeof(string))]
	class Boolean : IExpression
	{
		private readonly string _parameter;

		public string Operator => nameof(Boolean);
		public ExpressionResultType ResultType => ExpressionResultType.Boolean;

		public Boolean(string parameter)
		{
			_parameter = parameter;
		}

		public Task<InterpretationResult> InterpretAsync(IPlatformDatabase database, Dialog dialog)
		{
			return Task.FromResult(new InterpretationResult
			{
				ResultType = ExpressionResultType.Boolean,
				Result = true
			});
		}
	}
}
