using Counselor.Platform.Core.Behavior;

namespace Counselor.Platform.Repositories.Interfaces
{
	interface IBehaviorRepository
	{
		BehaviorIterator GetBehavior(string behaviorName);
	}
}
