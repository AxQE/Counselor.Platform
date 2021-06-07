namespace Counselor.Platform.Interpreter.Expressions
{
	public interface IExpressionFactory
	{
		IExpression CreateExpression(string instruction, string transport);
	}
}