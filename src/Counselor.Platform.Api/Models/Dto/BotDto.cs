using Counselor.Platform.Data.Entities.Enums;
using System.Text.Json.Serialization;

namespace Counselor.Platform.Api.Models.Dto
{
	public class BotDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public UserDto Owner { get; set; }

		public ScriptHeaderDto Script { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public BotState BotState { get; set; }

		public TransportDto Transport { get; set; }

		public string Configuration { get; set; }
	}
}
