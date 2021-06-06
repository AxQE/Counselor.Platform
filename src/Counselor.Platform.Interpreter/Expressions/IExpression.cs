using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions
{
	interface IExpression
	{
		string Operator { get; }
		Task<InterpretationResult> Interpret(IPlatformDatabase database, Dialog dialog);
	}
}
