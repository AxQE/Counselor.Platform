using System;

namespace Counselor.Platform.Core
{
	public interface IMessage
	{
		public Guid MessageId { get; set; }
		public int UserId { get; set; }
		public int Source { get; set; }
		public string Payload { get; set; }
	}
}
