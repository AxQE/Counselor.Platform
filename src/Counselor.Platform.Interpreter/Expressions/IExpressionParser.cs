namespace Counselor.Platform.Interpreter.Expressions
{
	public interface IExpressionParser
	{
		IExpression Parse(string instruction, string transport);
	}
}