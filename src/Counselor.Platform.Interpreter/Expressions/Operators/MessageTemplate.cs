using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Commands;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions.Operators
{
	[InterpreterCommand("regex", typeof(string))]
	internal class MessageTemplate : IExpression
	{
		public string Operator => throw new NotImplementedException();

		public Task<InterpretationResult> InterpretAsync(IPlatformDatabase database, Dialog dialog)
		{
			throw new NotImplementedException();
		}
	}
}
