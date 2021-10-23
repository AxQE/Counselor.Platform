using System.Collections.Generic;

namespace Counselor.Platform.Data.Entities
{
	public class Command : EntityBase
	{
		public string Name { get; set; }
		public Transport Transport { get; set; }
		public bool IsActive { get; set; }
		public IEnumerable<CommandParameter> Paramaters { get; set; }
	}
}
