using Counselor.Platform.Data.Entities.Enums;

namespace Counselor.Platform.Data.Entities
{
	public class Bot : EntityBase
	{
		public string Name { get; set; }
		public User Owner { get; set; }
		public Script Script { get; set; }
		public BotState BotState { get; set; }
		public Transport Transport { get; set; }		
		public string Configuration { get; set; }
	}
}
