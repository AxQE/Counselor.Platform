using System.Collections.Generic;

namespace Counselor.Platform.Data.Options
{
	public class TransportServiceOptions
	{
		public const string SectionName = "TransportService";
		public List<TransportOptions> Transports { get; set; }
		public int ServiceIntervalMs { get; set; } = 60000;
	}
}
