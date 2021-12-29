using System.Linq;

namespace Counselor.Platform.Core.Behavior
{
	static class BehaviorValidator
	{
		public static bool IsValid(BehaviorStep step)
		{
			return true;
		}

		public static bool IsValid(Behavior behavior)
		{
			if (behavior == null) return false;
			if (behavior.Steps.Count(x => x.IsRoot) > 1) return false;

			foreach (BehaviorStep step in behavior.Steps)
			{
				if (!IsValid(step)) return false;
			}

			return true;
		}
	}
}
