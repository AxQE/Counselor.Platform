using System.Collections.Generic;

namespace Counselor.Platform.Core.Behavior
{
	interface IBehaviorIterator
	{
		void Next(string processedId = default);
		IEnumerable<BehaviorStep> Current();
		void Reset();
	}
}
