using System.Collections.Generic;

namespace Counselor.Platform.Core.Behavior
{
	class Behavior
	{
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public List<BehaviorStep> Steps { get; set; }
		public BehaviorIterator Iterator => new BehaviorIterator(this);
	}
}
