using System.Linq.Expressions;

namespace Counselor.Platform.Interpreter.Expressions.Boolean
{
	interface ILogicalExpression
	{
		string Operator { get; }
		Associativity Associativity { get; }
		Priority Priority { get; }
		Expression GetExpression();
	}
}
