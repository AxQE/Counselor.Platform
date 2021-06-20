using Microsoft.CSharp.RuntimeBinder;
using System.Linq.Expressions;

namespace Counselor.Platform.Interpreter.Expressions
{
	//https://habr.com/ru/post/486972/
	static class ExpressionDynamicBuilder
	{
		public static Expression BuildBinaryLogicalOperation(ExpressionType type, Expression left, Expression right)
		{
			var argumentInfo = new[]
			{
				CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
				CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
			};

			var binder = Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, type, typeof(ExpressionDynamicBuilder), argumentInfo);
			return Expression.Dynamic(binder, typeof(object), left, right);
		}

		public static Expression BuildUnaryOperation(ExpressionType type, Expression arg)
		{
			var argumentInfo = new[]
			{
				CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
			};

			var binder = Binder.UnaryOperation(CSharpBinderFlags.None, type, typeof(ExpressionDynamicBuilder), argumentInfo);
			return Expression.Dynamic(binder, typeof(object), arg);
		}
	}
}
