using System.ComponentModel.DataAnnotations;

namespace Counselor.Platform.Api.Models.Requests
{
	public class BotCreateRequest
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public int ScriptId { get; set; }

		[Required]
		public int TransportId { get; set; }

		[Required]
		public string Configuration { get; set; }
	}
}
