using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Services;
using System;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorContext : IDisposable
	{
		public ServiceContext ServiceContext { get; set; }
		public BehaviorIterator Iterator { get; set; }
		public IPlatformDatabase Database { get; set; }
		public Dialog Dialog { get; set; }
		public User Client { get; set; }		
		public string ConnectionId { get; set; }		

		public void Dispose()
		{
			Database?.Dispose();
		}
	}
}
