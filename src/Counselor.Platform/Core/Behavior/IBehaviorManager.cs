namespace Counselor.Platform.Core.Behavior
{
	//todo: будет достаточно репозитория, в таком виде смысла нет, только путаницу вносит
	interface IBehaviorManager
	{
		IBehaviorIterator GetBehavior(string dialogName);
	}
}
