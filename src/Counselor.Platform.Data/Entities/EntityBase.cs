﻿using System;

namespace Counselor.Platform.Data.Entities
{
	public abstract class EntityBase
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ModifiedOn { get; set; }
	}
}
