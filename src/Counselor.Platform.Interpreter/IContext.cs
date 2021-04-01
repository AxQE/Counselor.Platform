using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Database;

namespace Counselor.Platform.Interpreter
{
	interface IContext
	{
		public IPlatformDatabase Database { get; }
		public Dialog Dialog { get; set; }
		public IInstruction Instruction { get; set; }

	}
}
