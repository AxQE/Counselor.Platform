using Counselor.Platform.Options;

namespace Counselor.Platform.Worker.Transport.Telegram
{
	class TelegramOptions : TransportOptions
	{
		public const string SectionName = "Telegram";
		public override bool IsEnabled { get; set; }
		public override bool SendErrorReport { get; set; }
		public override string TransportSystemName => SectionName;
		public override string DialogName { get; set; }
		public string Token { get; set; }
	}
}
