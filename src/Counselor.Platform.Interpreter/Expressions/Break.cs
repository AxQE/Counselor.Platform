using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions
{
	class Break : IExpression
	{
		public Task Interpret(IContext context)
		{
			throw new NotImplementedException();
		}
	}
}
