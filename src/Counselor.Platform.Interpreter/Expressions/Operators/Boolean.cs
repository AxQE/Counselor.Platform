using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions.Operators
{
	class Boolean : IExpression
	{
		private readonly string _parameter;

		public string Operator => nameof(Boolean);
		public ExpressionResultType ResultType => ExpressionResultType.Boolean;

		public Boolean(string parameter)
		{
			_parameter = parameter;
		}

		public async Task<InterpretationResult> InterpretAsync(IPlatformDatabase database, Dialog dialog)
		{
			return await Task.FromResult(new InterpretationResult
			{
				ResultType = ExpressionResultType.Boolean,
				Result = true
			});
		}
	}
}
