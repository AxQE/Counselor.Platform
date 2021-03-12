using System.Collections.Generic;

namespace Counselor.Platform.Core.Behavior
{
	class Behavior : IBehavior
	{
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public List<BehaviorStep> Steps { get; set; }
	}
}
