using Counselor.Platform.Core.Behavior.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorStep
	{
		public string Id { get; set; }
		public bool IsActive { get; set; } = true;
		public string FriendlyName { get; set; }
		public string Response { get; set; }
		public BehaviorStepType StepType { get; set; } = BehaviorStepType.External;
		public List<string> Transitions { get; set; }
		public BehaviorStepCondition Condition { get; set; }
		public BehaviorCommand Command { get; set; }
		public BehaviorCommand OnSuccess { get; set; }
		public BehaviorCommand OnFailure { get; set; }		
		public BehaviorCommand Before { get; set; }
		public BehaviorCommand After { get; set; }
	}
}
