using System;

namespace Counselor.Platform.Interpreter.Queries
{
	public interface IQueryBuilder
	{
		IQuery Build();
		void With(Func<string> func);
	}
}
