using Counselor.Platform.Api.Models.Dto;
using Counselor.Platform.Data.Entities;
using Mapster;

namespace Counselor.Platform.Api.Models.Mapping
{
	public class CommandMapper : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Command, CommandDto>()
				.Map(dest => dest.Transport, src => src.Transport.Name)
				.Map(dest => dest.IsActive, src => src.IsActive)
				.Map(dest => dest.Parameters, src => src.Paramaters);

			config.NewConfig<CommandParameter, CommandParameterDto>()
				.Map(dest => dest.Name, src => src.Name)
				.Map(dest => dest.TypeName, src => src.Type);
		}
	}
}
