namespace Counselor.Platform.Data.Entities
{
	public class DialogScript : EntityBase
	{
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public string Script { get; set; }
	}
}
