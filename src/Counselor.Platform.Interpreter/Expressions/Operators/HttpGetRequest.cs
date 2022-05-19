using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions.Operators
{
	internal class HttpGetRequest : IExpression
	{
		public string Operator => throw new NotImplementedException();

		public Task<InterpretationResult> InterpretAsync(IPlatformDatabase database, Dialog dialog)
		{
			throw new NotImplementedException();
		}
	}
}
