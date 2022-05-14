using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Api.Models.Factories;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
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

		public async Task<Envelope<IEnumerable<TransportDto>>> GetAllTransports(CancellationToken cancellationToken)
		{
			var transports = await _database.Transports.ToListAsync(cancellationToken);
			return EnvelopeFactory.Create<IEnumerable<TransportDto>>(HttpStatusCode.OK, transports);
		}

		public async Task<Envelope<TransportDto>> GetTransportById(int transportId, CancellationToken cancellationToken)
		{
			var transport = await _database.Transports.FirstOrDefaultAsync(x => x.Id == transportId, cancellationToken);
			return EnvelopeFactory.Create<TransportDto>(HttpStatusCode.OK, transport);
		}

		public async Task<Envelope<IEnumerable<InterpreterCommandDto>>> GetTranposportCommands(int transportId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
