using System;
using System.Collections.Generic;

namespace Counselor.Platform.Core.Behavior
{
	class Behavior : IBehavior
	{
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public List<BehaviorStep> Steps { get; set; }
		public IBehaviorIterator Iterator => new BehaviorIterator(this);		
	}
}
