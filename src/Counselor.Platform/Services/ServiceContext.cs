namespace Counselor.Platform.Services
{
	public class ServiceContext
	{
		public int BotId { get; init; }
		public int ScriptId { get; set; }
		public int OwnerId { get; init; }
		public string TransportName { get; set; }
		public int TransportId { get; set; }
	}
}
