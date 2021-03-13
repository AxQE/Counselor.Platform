using System;

namespace Counselor.Platform.Exceptions
{
	class BehaviorException : Exception
	{
		public BehaviorException() : base()
		{
		}

		public BehaviorException(string message) : base(message)
		{
		}

		public BehaviorException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
