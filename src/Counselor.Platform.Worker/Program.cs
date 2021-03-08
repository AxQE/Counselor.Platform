using Counselor.Platform.DependencyInjection;
using Counselor.Platform.Options;
using Counselor.Platform.Worker.Systems.Discord;
using Counselor.Platform.Worker.Systems.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Counselor.Platform.Worker
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, builder) =>
				{
					builder.AddYamlFile("platformsettings.yaml", optional: false, reloadOnChange: true);
				})
				.ConfigureServices((hostContext, services) =>
				{
					CreateConfigurations(hostContext, services);

					RegistrateHostedServices(services);
					RegistrateOutgoingServices(services);
					RegistratePlatformServices(hostContext, services);
				});

		private static void RegistratePlatformServices(HostBuilderContext hostContext, IServiceCollection services)
		{
			services.AddSingleton<TelegramOutgoingService>();
			var serviceProvider = services.BuildServiceProvider();

			PlatformInitializer.Initialize(
				services,
				hostContext,
				new[]
				{
					serviceProvider.GetRequiredService<TelegramOutgoingService>()
				}
						);
		}

		private static void RegistrateHostedServices(IServiceCollection services)
		{
			services.AddHostedService<TelegramWorker>();
			services.AddHostedService<DiscordWorker>();
		}

		private static void RegistrateOutgoingServices(IServiceCollection services)
		{
			services.BuildServiceProvider();
		}

		private static void CreateConfigurations(HostBuilderContext hostContext, IServiceCollection services)
		{
			services.AddOptions();
			services.Configure<TelegramOptions>(hostContext.Configuration.GetSection(TelegramOptions.SectionName));
			services.Configure<DiscordOptions>(hostContext.Configuration.GetSection(DiscordOptions.SectionName));
		}
	}
}