using Counselor.Platform.Core.Behavior.Enums;
using Counselor.Platform.Interpreter;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorInstruction : IInstruction
	{
		public string Name { get; set; }
		public BehaviorCommandType Type { get; set; } = BehaviorCommandType.Instruction;
		public string Data { get; set; }
		public string Instruction => Type == BehaviorCommandType.Instruction ? Data : null;
	}
}
