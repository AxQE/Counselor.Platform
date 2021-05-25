using System;

namespace Counselor.Platform.Interpreter
{
	public class InterpretationResult
	{
		public InterpretationResultState State { get; set; }
		public object Result { get; set; }

		/// <summary>
		/// Cast result object to the required type
		/// </summary>
		/// <exception cref="NotImplementedException">Cast operation was not implemented for the type.</exception>
		public TResult GetTypedResult<TResult>()
		{			
			switch (Type.GetTypeCode(typeof(TResult)))
			{				
				case TypeCode.Boolean:
					return (TResult)Result;

				case TypeCode.String:
					throw new NotImplementedException($"Cast operation was not implemented for the type: {typeof(TResult)}.");

				default:
					throw new NotImplementedException($"Cast operation was not implemented for the type: {typeof(TResult)}.");
			}			
		}
	}
}