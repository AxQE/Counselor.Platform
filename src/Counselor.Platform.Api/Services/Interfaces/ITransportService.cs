using Counselor.Platform.Api.Models;
using Counselor.Platform.Api.Models.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface ITransportService
	{
		Task<Envelope<IEnumerable<TransportDto>>> GetAllTransports(bool onlyActive, CancellationToken cancellationToken);
		Task<Envelope<TransportDto>> GetTransportById(int transportId, CancellationToken cancellationToken);
		Task<Envelope<IEnumerable<CommandDto>>> GetTranposportCommands(int transportId, CancellationToken cancellationToken);
	}
}
