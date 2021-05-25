using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter
{
	interface IExpression
	{
		string Operator { get; }
		Associativity Associativity { get; }
		Priority Priority { get; }
		Task<InterpretationResult> Interpret(IPlatformDatabase database, Dialog dialog);
	}
}
