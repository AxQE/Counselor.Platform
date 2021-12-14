using System;

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
