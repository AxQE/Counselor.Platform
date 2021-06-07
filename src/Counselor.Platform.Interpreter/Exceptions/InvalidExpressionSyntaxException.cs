using System;

namespace Counselor.Platform.Interpreter.Exceptions
{
	public class InvalidExpressionSyntaxException : Exception
	{
		public InvalidExpressionSyntaxException()
		{
		}

		public InvalidExpressionSyntaxException(string message) : base(message)
		{
		}

		public InvalidExpressionSyntaxException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
