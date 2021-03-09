using System.Collections.Generic;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorStep
	{
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public BehaviorStepWhen When { get; set; }
		public string Message { get; set; }
		public List<BehaviorStep> Next { get; set; }
	}

	enum BehaviorStepWhen
	{
		Never,
		Always
	}
}
