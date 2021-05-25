using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions
{
	class MessageConstains : IExpression
	{
		private readonly string _parameters;

		public string Operator => "MessageConstains";

		public Associativity Associativity => throw new NotImplementedException();

		public Priority Priority => throw new NotImplementedException();

		public MessageConstains(string parameters)
		{
			_parameters = parameters;
		}

		public async Task<InterpretationResult> Interpret(IPlatformDatabase database, Dialog dialog)
		{
			var parameters = _parameters.Split(' ').Select(x => x.Trim());

			var result = new InterpretationResult();

			foreach (var p in parameters)
			{
				if (dialog.CurrentMessage.Payload.Contains(p))
					result.Result = true;

			}

			return result;
		}		
	}
}
