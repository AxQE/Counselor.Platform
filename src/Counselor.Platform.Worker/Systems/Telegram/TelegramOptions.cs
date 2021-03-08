namespace Counselor.Platform.Worker.Systems.Telegram
{
	class TelegramOptions
	{
		public const string SectionName = "Telegram";
		public const string TransportSystemName = "Telegram";
		public bool IsEnabled { get; set; }
		public string Token { get; set; }
	}
}
