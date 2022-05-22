using System.Collections.Generic;

namespace Counselor.Platform.Api.Models.Dto
{
	public class CommandDto
	{
		public string Name { get; set; }
		public string Transport { get; set; }
		public bool IsActive { get; set; }
		public IEnumerable<CommandParameterDto> Parameters { get; set; }
	}

	public class CommandParameterDto
	{
		public string Name { get; set; }
		public string TypeName { get; set; }
	}
}
