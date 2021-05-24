using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Templates
{
	class TextTemplateHandler : ITextTemplateHandler
	{
		public TextTemplateHandler()
		{

		}

		public async Task<string> InsertEntityParameters(string messageTemplate)
		{
			return await Task.FromResult(messageTemplate);
		}

		private IEnumerable<string> GetTemplateParameters(string messageTemplate)
		{
			throw new NotImplementedException();
		}
	}
}
