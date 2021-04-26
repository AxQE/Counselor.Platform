using Counselor.Platform.Core.Pipeline;
using Counselor.Platform.Core.Pipeline.Steps;
using Counselor.Platform.Data.DependencyInjection;
using Counselor.Platform.Options;
using Counselor.Platform.Repositories;
using Counselor.Platform.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Counselor.Platform.DependencyInjection
{
	public static class PlatformInitializer
	{
		public static void Initialize(IServiceCollection services, HostBuilderContext hostContext)
		{
			services.AddMemoryCache();
			CreateConfigurations(services, hostContext);


			#region services
			services.AddSingleton<IOutgoingServicePool, OutgoingServicePool>();
			#endregion

			#region behavior

			#endregion

			#region repositories
			services.AddSingleton<ConnectionsRepository>();
			services.AddSingleton<DialogsRepository>();
			services.AddSingleton<BehaviorRepository>();
			#endregion

			#region pipeline
			services.AddTransient<IPipelineExecutor, PipelineExecutor>();
			services.AddTransient<PipelineBehaviorStep>();
			#endregion

			DatabaseInitializer.RegistrateDatabase(services, hostContext);
		}

		private static void CreateConfigurations(IServiceCollection services, HostBuilderContext hostContext)
		{
			services.AddOptions();
			services.Configure<CacheOptions>(hostContext.Configuration.GetSection(CacheOptions.SectionName));
			services.Configure<DatabaseOptions>(hostContext.Configuration.GetSection(DatabaseOptions.SectionName));
			services.Configure<PlatformOptions>(hostContext.Configuration.GetSection(PlatformOptions.SectionName));
		}
	}
}
