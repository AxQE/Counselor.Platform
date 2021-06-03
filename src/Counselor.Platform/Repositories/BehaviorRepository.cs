using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Exceptions;
using Counselor.Platform.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace Counselor.Platform.Repositories
{
	class BehaviorRepository : IBehaviorRepository
	{
		private readonly ILogger<BehaviorRepository> _logger;
		private readonly PlatformOptions _options;
		private readonly Dictionary<string, Behavior> _availableBehaviors = new Dictionary<string, Behavior>();

		public BehaviorRepository(
			ILogger<BehaviorRepository> logger,
			IOptions<PlatformOptions> options
			)
		{
			_logger = logger;
			_options = options.Value;
			FillAvailableDialogs();
		}

		public BehaviorIterator GetBehavior(string behaviorName)
		{
			if (!_availableBehaviors.TryGetValue(behaviorName, out var behavior))
			{
				throw new EntityNotFoundException($"Behavior not found by name: {behaviorName}.");
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
