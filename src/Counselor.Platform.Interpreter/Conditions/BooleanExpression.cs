using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Conditions
{
	abstract class BooleanExpression : IExpression
	{
		public string Operator => throw new NotImplementedException();

		public Associativity Associativity => throw new NotImplementedException();

		public Priority Priority => throw new NotImplementedException();

		public Task<InterpretationResult> Interpret(IPlatformDatabase database, Dialog dialog)
		{
			throw new NotImplementedException();
		}
	}
}
