using System.Collections.Generic;

namespace Counselor.Platform.Core.Behavior
{
	interface IBehavior
	{
		string Name { get; }
		bool IsActive { get; }
		List<BehaviorStep> Steps { get; }
		BehaviorIterator Iterator { get; }
	}
}
