using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using System;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorContext : IDisposable
	{
		public Services.ServiceContext ServiceContext { get; set; }
		public BehaviorIterator Iterator { get; set; }
		public IPlatformDatabase Database { get; set; }
		public Dialog Dialog { get; set; }
		public User Client { get; set; }
		public string ConnectionId { get; set; }
		public DateTime CreatedOn { get; } = DateTime.Now;
		public DateTime LastUsedOn { get; set; }

		public void Dispose()
		{
			Database?.Dispose();
		}
	}
}
