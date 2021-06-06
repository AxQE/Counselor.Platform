using Counselor.Platform.Interpreter.Expressions;

namespace Counselor.Platform.Interpreter.Expressions.Boolean
{
	class NotEqual : BooleanExpression
	{
		public NotEqual(IExpression left, IExpression right) 
			: base("NotEqual", Associativity.Left, Priority.Comparison, left, right)
		{
		}
	}
}
