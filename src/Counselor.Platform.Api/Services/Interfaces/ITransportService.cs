using Counselor.Platform.Api.Entities.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Api.Services.Interfaces
{
	public interface ITransportService
	{
		Task<IEnumerable<TransportDto>> GetAllTransports();
		Task<TransportDto> GetTransportById(int transportId);
	}
}
