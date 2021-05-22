using Counselor.Platform.Data.Database;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Utils
{
	class MessageTemplateHandler : IMessageTemplateHandler
	{		
		private readonly IMemoryCache _cache;
		private readonly IPlatformDatabase _database;

		public MessageTemplateHandler(IMemoryCache cache, IPlatformDatabase database)
		{
			_cache = cache;
			_database = database;
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
