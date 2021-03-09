using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorManager : IBehaviorManager
	{
		private readonly IBehaviorBuilder _behaviorBuilder;
		private readonly ILogger<BehaviorManager> _logger;		

		public BehaviorManager(ILogger<BehaviorManager> logger, IBehaviorBuilder behaviorBuilder)
		{
			_logger = logger;
			_behaviorBuilder = behaviorBuilder;
		}

		public IBehavior GetBehavior()
		{
			throw new NotImplementedException();
		}
	}
}
