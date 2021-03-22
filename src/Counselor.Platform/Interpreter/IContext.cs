using Counselor.Platform.Database;
using Counselor.Platform.Entities;

namespace Counselor.Platform.Interpreter
{
	interface IContext
	{
		public IPlatformDatabase Database { get; }
		public Dialog Dialog { get; set; }
		public IInstruction Instruction { get; set; }

	}
}
