using Counselor.Platform.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorManager : IBehaviorManager
	{		
		private readonly ILogger<BehaviorManager> _logger;
		private readonly PlatformOptions _options;
		private readonly Dictionary<string, Behavior> _availableBehaviors = new Dictionary<string, Behavior>();

		public BehaviorManager(
			ILogger<BehaviorManager> logger, 			
			IOptions<PlatformOptions> options
			)
		{
			_logger = logger;			
			_options = options.Value;
			FillAvailableDialogs();
		}

		public IBehaviorIterator GetBehavior(string behaviorName)
		{
			if (!_availableBehaviors.TryGetValue(behaviorName, out var behavior))
			{
				throw new ArgumentOutOfRangeException(nameof(behaviorName), $"Behavior not found by name: {behaviorName}.");
			}

			return behavior.Iterator;
		}

		private void FillAvailableDialogs()
		{
			var dialogsPath = _options.DialogsPath;

			if (Directory.Exists(dialogsPath))
			{
				var deserializer = new YamlDotNet.Serialization.Deserializer();

				foreach (var dialogFile in Directory.GetFiles(dialogsPath, "*.yaml", SearchOption.AllDirectories))
				{
					using (var reader = new StreamReader(dialogFile))
					{						
						var dialog = deserializer.Deserialize<Behavior>(reader.ReadToEnd());
						_availableBehaviors.Add(dialog.Name, dialog);
					}
				}
			}
		}
	}
}
