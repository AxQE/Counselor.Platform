using Counselor.Platform.Core.Behavior.Enums;
using System;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorCommand
	{
		public string Name { get; set; }
		public BehaviorCommandType Type { get; set; }
		public string Data { get; set; }
	}
}
