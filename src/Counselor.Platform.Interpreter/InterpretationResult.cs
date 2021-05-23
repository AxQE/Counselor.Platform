using System;
using System.Collections.Generic;

namespace Counselor.Platform.Interpreter
{	
	public class InterpretationResult
	{
		/// <summary>
		/// Cast result object to the required type
		/// </summary>
		/// <exception cref="NotImplementedException">Cast operation was not implemented for the type.</exception>
		public TResult GetTypedResult<TResult>()
		{
			switch (Type.GetTypeCode(typeof(TResult)))
			{
				case TypeCode.Boolean:
					return (TResult)Convert.ChangeType(true, TypeCode.Boolean);
				default:
					throw new NotImplementedException($"Cast operation was not implemented for the type: {typeof(TResult)}.");
			}			
		}
	}
}