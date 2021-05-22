using Counselor.Platform.Core.Behavior.Enums;
using System.Collections.Generic;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorStep
	{
		public string Id { get; set; }
		public bool IsActive { get; set; } = true;
		public bool IsRoot { get; set; } = false;
		public string FriendlyName { get; set; }
		public string Response { get; set; }
		public BehaviorStepType StepType { get; set; } = BehaviorStepType.External;
		public List<string> Transitions { get; set; }
		public BehaviorInstruction Condition { get; set; }
		public BehaviorInstruction Command { get; set; }
		public BehaviorInstruction OnSuccess { get; set; }
		public BehaviorInstruction OnFailure { get; set; }
		public BehaviorInstruction Before { get; set; }
		public BehaviorInstruction After { get; set; }
	}
}
