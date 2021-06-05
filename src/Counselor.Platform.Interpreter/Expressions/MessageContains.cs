using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions
{
	class MessageContains : IExpression
	{
		private readonly string _parameters;

		public string Operator => nameof(MessageContains);

		public MessageContains(string parameters)
		{
			_parameters = parameters;
		}

		public async Task<InterpretationResult> Interpret(IPlatformDatabase database, Dialog dialog)
		{
			var parameters = _parameters.Split(' ').Select(x => x.Trim());

			var result = new InterpretationResult
			{
				ResultType = InterpretationResultType.InternalCommand
			};

			foreach (var p in parameters)
			{
				if (dialog.CurrentMessage.Payload.Contains(p))
					result.Result = true;
			}

			return result;
		}		
	}
}
