using Counselor.Platform.Data.Options;

namespace Counselor.Platform.Worker.Transport.Discord
{
	class DiscordOptions : TransportOptions
	{
		public const string SectionName = "Discord";
		public override bool IsEnabled { get; set; }
		public override bool SendErrorReport { get; set; }
		public override string DialogName { get; set; }
		public override string SystemName => SectionName;
	}
}
