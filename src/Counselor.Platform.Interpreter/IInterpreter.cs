using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter
{
	public interface IInterpreter : IDisposable
	{
		Task<InterpretationResult> Interpret(IInstruction instruction, Dialog dialog, IPlatformDatabase database);
		Task<string> FillEntityParameters(string message, Dialog dialog, IPlatformDatabase database);
	}
}
