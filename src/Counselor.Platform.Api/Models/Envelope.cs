using System.Net;
using System.Text.Json.Serialization;

namespace Counselor.Platform.Api.Models
{
	public record Envelope<TData> where TData : class
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public TData Data { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Message { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Error Error { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public HttpStatusCode HttpStatus { get; set; }
	}
}
