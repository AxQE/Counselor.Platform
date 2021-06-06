using Counselor.Platform.Interpreter.Expressions;

namespace Counselor.Platform.Interpreter.Expressions.Boolean
{
	class Or : BooleanExpression
	{
		public Or(IExpression left, IExpression right) 
			: base("Or", Associativity.Left, Priority.LogicalAddition, left, right)
		{
		}
	}
}
