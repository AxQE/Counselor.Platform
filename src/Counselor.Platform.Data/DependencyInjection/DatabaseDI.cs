using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Counselor.Platform.Data.DependencyInjection
{
	public static class DatabaseDI
	{
		public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
		{
			var dbOptions = new DatabaseOptions();
			configuration.GetSection(DatabaseOptions.SectionName).Bind(dbOptions);

			services.AddDbContext<IPlatformDatabase, PlatformDbContext>(options =>
				options.UseNpgsql(dbOptions.BuildConnectionString()).UseSnakeCaseNamingConvention(),
				ServiceLifetime.Transient,
				ServiceLifetime.Transient
				);
		}
	}
}
