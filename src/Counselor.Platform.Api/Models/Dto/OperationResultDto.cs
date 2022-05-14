namespace Counselor.Platform.Api.Models.Dto
{
	public class OperationResultDto
	{
		public OperationResult Result { get; set; }
	}

	public enum OperationResult
	{
		Failed,
		Success
	}
}
