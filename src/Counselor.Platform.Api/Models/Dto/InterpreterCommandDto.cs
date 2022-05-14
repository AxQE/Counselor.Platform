using System.Collections.Generic;

namespace Counselor.Platform.Api.Models.Dto
{
	public class InterpreterCommandDto
	{
		public string Name { get; set; }
		public CommandType CommandType { get; set; }
		public string Transport { get; set; }
		public bool IsActive { get; set; }
		public IEnumerable<InterpreterCommandParameterDto> Parameters { get; set; }
	}

	public class InterpreterCommandParameterDto
	{
		public string Name { get; set; }
		public string TypeName { get; set; }
	}

	public enum CommandType
	{
		Internal,
		External
	}
}
