using System;
using System.Collections.Generic;
using System.Linq;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorIterator : IBehaviorIterator
	{
		private readonly IBehavior _behavior;
		private readonly IReadOnlyCollection<BehaviorStep> _root;
		private IEnumerable<BehaviorStep> _current;

		public BehaviorIterator(IBehavior behavior)
		{
			_behavior = behavior;

			if (behavior?.Steps is null)
				throw new ArgumentNullException($"Behavior steps was not created.");

			var rootNode = behavior.Steps.FirstOrDefault(x => x.IsRoot);

			if (rootNode is null)
				throw new ArgumentNullException($"Root step not found for dialog: {behavior.Name}.");

			_root = new List<BehaviorStep> { rootNode };

			_current = _root;
		}

		public IEnumerable<BehaviorStep> Current()
		{
			return _current;
		}

		public void Next(string processedId = null)
		{
			//todo: пальцы быстрее мыслей. как то странно получилось нужно посмотреть потом
			if (!string.IsNullOrEmpty(processedId))
			{
				var processed = _behavior.Steps.First(x => x.Id.Equals(processedId)).Transitions;

				if (processed?.Any() ?? false)
					_current = _behavior.Steps.Where(x => processed.Contains(x.Id)).ToList();
				else
					_current = null;
			}
			else
			{
				var nextStepIds = _current
					?.Where(s => s?.Transitions?.Any() ?? false)
					?.SelectMany(x => x.Transitions)?.ToList();

				if (nextStepIds?.Any() ?? false)
				{
					var next = _behavior.Steps.Where(x => nextStepIds.Contains(x.Id));
					if (next.Any())
						_current = next.ToList();
					else
						_current = null;
				}
				else
				{
					_current = null;
				}
			}
		}

		public void Reset()
		{
			_current = _root;
		}
	}
}
