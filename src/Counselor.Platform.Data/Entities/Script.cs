namespace Counselor.Platform.Data.Entities
{
	public class Script : EntityBase
	{
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public string Data { get; set; }
	}
}
