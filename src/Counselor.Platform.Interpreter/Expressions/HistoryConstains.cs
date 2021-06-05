using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions
{
	class HistoryConstains : IExpression
	{
		private readonly string _parameters;

		public string Operator => nameof(HistoryConstains);

		public HistoryConstains(string parameters)
		{
			_parameters = parameters;
		}

		public async Task<InterpretationResult> Interpret(IPlatformDatabase database, Dialog dialog)
		{
			throw new NotImplementedException();
		}
	}
}
