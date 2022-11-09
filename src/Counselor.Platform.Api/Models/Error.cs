using System;

namespace Counselor.Platform.Api.Models
{
	public class Error
	{
		public Guid? Id { get; set; }
		public int Code { get; set; }
		public string Message { get; set; }
	}
}
