using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter
{
	interface IExpression
	{
		Task Interpret(IContext context);
	}
}
