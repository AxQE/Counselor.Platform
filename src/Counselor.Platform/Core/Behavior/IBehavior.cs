using System.Collections.Generic;

namespace Counselor.Platform.Core.Behavior
{
	interface IBehavior
	{
		public string Name { get; }
		public bool IsActive { get; }
		public List<BehaviorStep> Steps { get; }
	}
}
