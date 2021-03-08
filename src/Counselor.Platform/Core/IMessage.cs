using System;

namespace Counselor.Platform.Core
{
	public interface IMessage
	{
		public Guid Id { get; set; }
		public string Payload { get; set; }
	}
}
