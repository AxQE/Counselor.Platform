namespace Counselor.Platform.Core
{
	public class PipelineResult
	{
		public bool SuccessfullyCompleted { get; set; }
		public int ErrorCode { get; set; }

		public PipelineResult()
		{
			SuccessfullyCompleted = true;
		}		
	}
}
