using System;
using System.Runtime.Serialization;

namespace Counselor.Platform.Interpreter.Exceptions
{
	class InterpretationGenericException : Exception
	{
		public InterpretationGenericException()
		{
		}

		public InterpretationGenericException(string message) : base(message)
		{
		}

		public InterpretationGenericException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InterpretationGenericException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
