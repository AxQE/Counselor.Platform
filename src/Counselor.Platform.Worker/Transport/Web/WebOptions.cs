using Counselor.Platform.Data.Options;

namespace Counselor.Platform.Worker.Transport.Web
{
	public class WebOptions : TransportOptions
	{
		public const string SectionName = "Web";

		public override string SystemName => SectionName;
	}
}
