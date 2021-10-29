using System.Collections.Generic;

namespace Counselor.Platform.Data.Entities
{
	public class Command : EntityBase
	{
		public string Name { get; set; }
		public Transport Transport { get; set; }
		public bool IsActive { get; set; }
		public List<CommandParameter> Paramaters { get; set; }
	}
}
