namespace Counselor.Platform.Core.Behavior
{
	interface IBehaviorManager
	{
		IBehavior GetBehavior(string dialogName);
	}
}
