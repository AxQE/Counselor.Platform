using Counselor.Platform.Core.Behavior.Enums;

namespace Counselor.Platform.Core.Behavior
{
	interface IBehaviorCommand
	{
		public string Name { get; set; }
		public BehaviorCommandType Type { get; set; }
		public string Data { get; set; }
	}
}
