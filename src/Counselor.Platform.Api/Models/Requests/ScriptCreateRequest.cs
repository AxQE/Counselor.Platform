using System.ComponentModel.DataAnnotations;

namespace Counselor.Platform.Api.Models.Requests
{
	public class ScriptCreateRequest
	{
		[Required]
		public string Name { get; set; }
		public string Instruction { get; set; }
	}
}
