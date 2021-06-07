using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions
{
	public interface IExpression
	{
		string Operator { get; }
		Task<InterpretationResult> InterpretAsync(IPlatformDatabase database, Dialog dialog);
	}
}
