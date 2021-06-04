using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Exceptions;
using Counselor.Platform.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Counselor.Platform.Tests")]

namespace Counselor.Platform.Repositories
{
	class BehaviorRepository : IBehaviorRepository
	{
		private readonly ILogger<BehaviorRepository> _logger;
		private readonly PlatformOptions _options;
		private readonly Dictionary<string, Behavior> _availableBehaviors = new Dictionary<string, Behavior>();

		public BehaviorRepository(
			ILogger<BehaviorRepository> logger,
			IOptions<PlatformOptions> options,
			IPlatformDatabase database			
			)
		{
			_logger = logger;
			_options = options.Value;
			FillAvailableDialogs(database);
		}

		public BehaviorIterator GetBehavior(string behaviorName)
		{
			if (!_availableBehaviors.TryGetValue(behaviorName, out var behavior))
			{
				throw new EntityNotFoundException($"Behavior not found by name: {behaviorName}.");
			}

			return behavior.Iterator;
		}

		private void FillAvailableDialogs(IPlatformDatabase database)
		{
			var dialogsPath = _options.DialogsPath;
			var deserializer = new YamlDotNet.Serialization.Deserializer();

			foreach (var script in database.DialogScripts)
			{
				using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(script.Script)))
				using (var reader = new StreamReader(stream))
				{
					var dialog = deserializer.Deserialize<Behavior>(reader.ReadToEnd());
					_availableBehaviors.Add(dialog.Name, dialog);					
				}
			}
						
			if (Directory.Exists(dialogsPath))
			{
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
