using Counselor.Platform.Interpreter.Expressions;
using System.Linq.Expressions;

namespace Counselor.Platform.Interpreter.Expressions.Boolean
{
	class Not : ILogicalExpression
	{
		public string Operator => "Not";
		public Associativity Associativity => Associativity.Right;
		public Priority Priority => Priority.LogicalNegation;
		public IExpression Operand { get; }
		public Not(IExpression expression)
		{
			Operand = expression;
		}

		public Expression GetExpression()
		{
			return ExpressionDynamicBuilder.BuildUnaryOperation(ExpressionType.Not, null);
		}
	}
}
