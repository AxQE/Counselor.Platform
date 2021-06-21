using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services
{
	public class TransportService : ITransportService
	{
		private readonly IPlatformDatabase _database;
		private readonly ILogger<TransportService> _logger;

		public TransportService(
			IPlatformDatabase database, 
			ILogger<TransportService> logger
			)
		{
			_database = database;
			_logger = logger;
		}

		public async Task<IEnumerable<TransportDto>> GetAllTransports()
		{
			var transports = await _database.Transports.ToListAsync();
			return transports.Adapt<IEnumerable<TransportDto>>();
		}
	}
}
