using Counselor.Platform.Core.Behavior;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Counselor.Platform.Tests")]

namespace Counselor.Platform.Repositories.Interfaces
{	
	interface IBehaviorRepository
	{
		BehaviorIterator GetBehavior(string behaviorName);
	}
}
