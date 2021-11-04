using System.Collections.Generic;

namespace Counselor.Platform.Data.Options
{
	public class ServiceOptions
	{
		public const string SectionName = "Service";
		public List<TransportOptions> Transports { get; set; }
		public int ServiceIntervalMs { get; set; } = 60000;
		public int DialogTTLMs { get; set; } = 300000;
	}
}
