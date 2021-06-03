using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Queries
{
	class PostgresQueryBuilder : IQueryBuilder
	{
		public IQuery Build()
		{
			throw new NotImplementedException();
		}

		public void With(Func<string> func)
		{
			throw new NotImplementedException();
		}
	}
}
