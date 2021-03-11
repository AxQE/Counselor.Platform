using System;

namespace Counselor.Platform.Entities
{
	public class EntityBase
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ModifiedOn { get; set; }
	}
}
