using Counselor.Platform.Core;
using Counselor.Platform.Database;
using Counselor.Platform.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Counselor.Platform.DependencyInjection
{
	public static class PlatformInitializer
	{
		public static void Initialize(IServiceCollection services, HostBuilderContext hostContext, IReadOnlyCollection<IOutgoingService> outgoingServices)
		{
			services.AddMemoryCache();
			CreateConfigurations(services, hostContext);

			services.AddDbContext<IApplicationDatabase, ApplicationDbContext>(options =>
				options.UseNpgsql(hostContext.Configuration.GetConnectionString("Platform:Database")).UseSnakeCaseNamingConvention());

			services.AddSingleton<IOutgoingServicesPool, OutgoingServicePool>();

			RegistratePoolServices(services, outgoingServices);
		}

		private static void RegistratePoolServices(IServiceCollection services, IReadOnlyCollection<IOutgoingService> outgoingServices)
		{
			var pool = services.BuildServiceProvider().GetRequiredService<IOutgoingServicesPool>();

			foreach (var outgoingService in outgoingServices)
			{
				pool.Register(outgoingService);
			}
		}

		private static void CreateConfigurations(IServiceCollection services, HostBuilderContext hostContext)
		{			
			services.Configure<CacheOptions>(hostContext.Configuration.GetSection(CacheOptions.SectionName));
		}
	}
}
