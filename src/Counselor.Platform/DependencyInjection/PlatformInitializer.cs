using Counselor.Platform.Core.Pipeline;
using Counselor.Platform.Database;
using Counselor.Platform.Options;
using Counselor.Platform.Repositories;
using Counselor.Platform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Counselor.Platform.DependencyInjection
{
	public static class PlatformInitializer
	{
		public static void Initialize(IServiceCollection services, HostBuilderContext hostContext)
		{
			services.AddMemoryCache();
			CreateConfigurations(services, hostContext);

			services.AddSingleton<IOutgoingServicePool, OutgoingServicePool>();			
			services.AddSingleton<ConnectionsRepository>();
			services.AddSingleton<DialogsRepository>();

			services.AddTransient<IPipelineExecutor, PipelineExecutor>();

			RegistrateDatabase(services, hostContext);
		}

		public static void RegistratePoolServices(IServiceCollection services, IReadOnlyCollection<IOutgoingService> outgoingServices)
		{
			var pool = services.BuildServiceProvider().GetRequiredService<IOutgoingServicePool>();

			foreach (var outgoingService in outgoingServices)
			{
				pool.Register(outgoingService);
			}
		}

		private static void CreateConfigurations(IServiceCollection services, HostBuilderContext hostContext)
		{
			services.AddOptions();
			services.Configure<CacheOptions>(hostContext.Configuration.GetSection(CacheOptions.SectionName));
			services.Configure<DatabaseOptions>(hostContext.Configuration.GetSection(DatabaseOptions.SectionName));
		}

		private static void RegistrateDatabase(IServiceCollection services, HostBuilderContext hostContext)
		{
			var dbOptions = new DatabaseOptions();
			hostContext.Configuration.GetSection(DatabaseOptions.SectionName).Bind(dbOptions);

			services.AddDbContext<IPlatformDatabase, PlatformDbContext>(options =>
				options.UseNpgsql(dbOptions.BuildConnectionString()).UseSnakeCaseNamingConvention(),
				ServiceLifetime.Transient,
				ServiceLifetime.Transient
				);
		}
	}
}
