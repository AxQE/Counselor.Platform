using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter
{
	public interface IInterpreter : IDisposable
	{
		Task<InterpretationResult> Interpret(IInstruction instruction, Dialog dialog, IPlatformDatabase database, string transport);
		Task<InterpretationResult> Interpret(InterpretationContext context);
		Task<string> InsertEntityParameters(string message, Dialog dialog, IPlatformDatabase database);
	}
}
