using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Interpreter.Exceptions;
using System;

namespace Counselor.Platform.Interpreter
{
	public class InterpretationResult
	{
		public InterpretationResultState State { get; set; }
		public InterpretationResultType ResultType { get; set; }
		public object Result { get; set; }
		public ITransportCommand Command { get; set; }

		/// <summary>
		/// Cast elementary result object to the required type
		/// </summary>
		/// <exception cref="NotImplementedException">Cast operation was not implemented for the type.</exception>
		/// <exception cref="InterpretationGenericException">Trying to cast non elementary type as elementary</exception>
		public TResult GetTypedResult<TResult>()
		{
			if (ResultType != InterpretationResultType.Elementary)
				throw new InterpretationGenericException($"Interpretation result type is not elementary. ResultType: {ResultType}.");

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