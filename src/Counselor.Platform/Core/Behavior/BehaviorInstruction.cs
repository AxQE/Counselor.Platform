using Counselor.Platform.Core.Behavior.Enums;
using Counselor.Platform.Interpreter;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorInstruction : IInstruction
	{
		public BehaviorInstructionType Type { get; set; } = BehaviorInstructionType.Instruction;
		public string Data { get; set; }
		public string Instruction => Type == BehaviorInstructionType.Instruction ? Data : null;
	}
}
