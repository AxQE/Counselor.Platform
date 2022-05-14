using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface ITransportService
	{
		Task<Envelope<IEnumerable<TransportDto>>> GetAllTransports(CancellationToken cancellationToken);
		Task<Envelope<TransportDto>> GetTransportById(int transportId, CancellationToken cancellationToken);
	}
}
