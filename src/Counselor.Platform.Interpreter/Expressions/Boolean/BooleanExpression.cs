using System;
using System.Linq.Expressions;

namespace Counselor.Platform.Interpreter.Expressions.Boolean
{
	abstract class BooleanExpression : ILogicalExpression
	{
		public string Operator { get; }
		public Associativity Associativity { get; }
		public Priority Priority { get; }
		public IExpression Left { get; }
		public IExpression Right { get; }

		public BooleanExpression(string @operator, Associativity associativity, Priority priority, IExpression left, IExpression right)
		{
			Operator = @operator;
			Associativity = associativity;
			Priority = priority;
			Left = left;
			Right = right;
		}

		public Expression GetExpression()
		{
			throw new NotImplementedException();
		}
	}
}
