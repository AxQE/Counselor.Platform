using Counselor.Platform.Interpreter.Expressions;

namespace Counselor.Platform.Interpreter.Expressions.Boolean
{
	class Equal : BooleanExpression
	{
		public Equal(IExpression left, IExpression right) 
			: base("Equal", Associativity.Left, Priority.Comparison, left, right)
		{
		}
	}
}
