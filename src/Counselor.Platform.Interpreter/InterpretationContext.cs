using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;

namespace Counselor.Platform.Interpreter
{
	public struct InterpretationContext
	{
		public IPlatformDatabase Database { get; init; }
		public Dialog Dialog { get; init; }
		public IInstruction Instruction { get; init; }
		public string Transport { get; init; }
	}
}
