using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Commands;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions.Operators
{
	[InterpreterCommand("url", typeof(string))]
	[InterpreterCommand("body", typeof(object))]
	internal class HttpPostRequest : IExpression
	{
		private readonly string _url;
		private readonly object _body;
		public string Operator => nameof(HttpPostRequest);

		public HttpPostRequest(string url, object body)
		{
			_url = url;
			_body = body;
		}

		public Task<InterpretationResult> InterpretAsync(IPlatformDatabase database, Dialog dialog)
		{
			throw new NotImplementedException();
		}
	}
}
