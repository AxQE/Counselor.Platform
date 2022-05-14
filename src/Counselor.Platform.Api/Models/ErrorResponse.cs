using System;

namespace Counselor.Platform.Api.Models
{
	public class ErrorResponse
	{
		public Guid? Id { get; set; }
		public string Error { get; set; }
	}
}
