using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Counselor.Platform.Data.DependencyInjection
{
	public static class DatabaseInitializer
	{
		public static void RegistrateDatabase(IServiceCollection services, HostBuilderContext hostContext)
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
