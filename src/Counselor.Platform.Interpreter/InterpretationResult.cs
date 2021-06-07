using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Interpreter.Expressions;
using System;

namespace Counselor.Platform.Interpreter
{
	public class InterpretationResult
	{
		public InterpretationResultState State { get; set; } = InterpretationResultState.Completed;
		public ExpressionResultType ResultType { get; set; }
		public object Result { get; set; }
		public ITransportCommand Command { get; set; }

		/// <summary>
		/// Cast elementary result object to the required type
		/// </summary>
		/// <exception cref="NotImplementedException">Cast operation was not implemented for the type.</exception>
		public TResult GetTypedResult<TResult>()
		{
			switch (Type.GetTypeCode(typeof(TResult)))
			{				
				case TypeCode.Boolean:
					return (TResult)Result;

				default:
					throw new NotImplementedException($"Cast operation is not implemented for the type: {typeof(TResult)}.");
			}			
		}
	}
}