using System;
using System.Collections.Generic;
using System.Linq;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorIterator
	{
		private readonly Behavior _behavior;
		private readonly IReadOnlyCollection<BehaviorStep> _rootNode;
		private readonly Dictionary<string, IEnumerable<BehaviorStep>> _avalilableTransitions = new();
		private IEnumerable<BehaviorStep> _currentTransitions;

		public BehaviorIterator(Behavior behavior)
		{
			_behavior = behavior;

			if (behavior?.Steps is null)
				throw new ArgumentNullException("Behavior steps was not created.");

			var rootNode = behavior.Steps.Find(x => x.IsRoot);

			if (rootNode is null)
				throw new ArgumentNullException($"Root step not found for dialog: {behavior.Name}.");

			_rootNode = new BehaviorStep[1] { rootNode };
			_currentTransitions = _rootNode;

			foreach (var step in behavior.Steps)
			{
				_avalilableTransitions.Add(step.Id, FindAvailableTransitions(step));
			}
		}

		public IEnumerable<BehaviorStep> Current()
		{
			return _currentTransitions;
		}

		public void Next(string processedStepId)
		{
			if (!string.IsNullOrEmpty(processedStepId) && _avalilableTransitions.ContainsKey(processedStepId))
			{
				_currentTransitions = _avalilableTransitions[processedStepId];
			}
			else
			{
				_currentTransitions = null;
			}
		}

		public void Reset()
		{
			_currentTransitions = _rootNode;
		}

		private IEnumerable<BehaviorStep> FindAvailableTransitions(BehaviorStep step)
		{
			return _behavior.Steps.Where(x => x.IsActive && step?.Transitions != null && step.Transitions.Contains(x.Id));
		}
	}
}
