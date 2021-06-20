using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Queries
{
	public interface IQueryBuilder
	{		
		IQuery Build();
		void With(Func<string> func);
	}
}
