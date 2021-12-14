using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Counselor.Platform.Data.DependencyInjection
{
	public static class DatabaseDI
	{
		public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Transient)
		{
			var dbOptions = new DatabaseOptions();
			configuration.GetSection(DatabaseOptions.SectionName).Bind(dbOptions);

			services.AddDbContext<IPlatformDatabase, PlatformDbContext>(options =>
				options.UseNpgsql(dbOptions.BuildConnectionString()).UseSnakeCaseNamingConvention(),
				lifetime,
				lifetime
				);
		}
	}
}
