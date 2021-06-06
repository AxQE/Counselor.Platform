using Counselor.Platform.Interpreter.Expressions;

namespace Counselor.Platform.Interpreter.Expressions.Boolean
{
	class And : BooleanExpression
	{
		public And(IExpression left, IExpression right) 
			: base("And", Associativity.Left, Priority.LogicalMultiplication, left, right)
		{
		}
	}
}
