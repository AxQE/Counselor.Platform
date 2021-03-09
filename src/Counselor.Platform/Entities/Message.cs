using System;

namespace Counselor.Platform.Entities
{
	public class Message : EntityBase
	{
		public Guid Id { get; set; }
		public string Payload { get; set; }
	}
}
