using System;
using System.Net;
using System.Runtime.Serialization;

namespace Counselor.Platform.Api.Exceptions
{
	public class GenericApiException : Exception
	{
		public virtual int HttpStatusCode { get; protected set; }
		public virtual int? ErrorCode { get; protected set; }
		public virtual string ErrorReason { get; protected set; }

		public GenericApiException(int httpStatusCode, int? errorCode = default, string errorReason = default)
		{
			HttpStatusCode = httpStatusCode;
			ErrorCode = errorCode;
			ErrorReason = errorReason;
		}

		public GenericApiException(HttpStatusCode statusCode, int? errorCode = default, string errorReason = default)
		{
			HttpStatusCode = (int)statusCode;
			ErrorCode = errorCode;
			ErrorReason = errorReason;
		}

		public GenericApiException(string message, int httpStatusCode, int? errorCode = default, string errorReason = default) : base(message)
		{
			HttpStatusCode = httpStatusCode;
			ErrorCode = errorCode;
			ErrorReason = errorReason;
		}

		public GenericApiException(string message, Exception innerException, int httpStatusCode, int? errorCode = default, string errorReason = default) : base(message, innerException)
		{
			HttpStatusCode = httpStatusCode;
			ErrorCode = errorCode;
			ErrorReason = errorReason;
		}

		protected GenericApiException(SerializationInfo info, StreamingContext context, int httpStatusCode, int? errorCode = default, string errorReason = default) : base(info, context)
		{
			HttpStatusCode = httpStatusCode;
			ErrorCode = errorCode;
			ErrorReason = errorReason;
		}
	}
}
