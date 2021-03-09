using Counselor.Platform.Options;

namespace Counselor.Platform.Worker.Systems.Telegram
{
	class TelegramOptions : TransportOptions
	{
		public const string SectionName = "Telegram";
		public const string TransportName = "Telegram";
		public override bool IsEnabled { get; set; }
		public string Token { get; set; }
		public override string TransportSystemName => TransportName;
	}
}
