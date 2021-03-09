using Counselor.Platform.Options;

namespace Counselor.Platform.Worker.Transport.Discord
{
	class DiscordOptions : TransportOptions
	{
		public const string SectionName = "Discord";
		public override bool IsEnabled { get; set; }
		public override string TransportSystemName => SectionName;
	}
}
