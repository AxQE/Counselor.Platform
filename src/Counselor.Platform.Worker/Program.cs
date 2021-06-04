using Counselor.Platform.DependencyInjection;
using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Services;
using Counselor.Platform.Worker.Transport.Discord;
using Counselor.Platform.Worker.Transport.Discord.Commands;
using Counselor.Platform.Worker.Transport.Telegram;
using Counselor.Platform.Worker.Transport.Telegram.Commands;
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

					if (context.HostingEnvironment.IsDevelopment())
					{
						builder.AddUserSecrets<Program>();
					}
				})
				.ConfigureServices((hostContext, services) =>
				{
					CreateConfigurations(hostContext, services);

					RegistrateHostedServices(services);
					RegistratePlatformServices(hostContext, services);
				});

		private static void RegistratePlatformServices(HostBuilderContext hostContext, IServiceCollection services)
		{
			PlatformInitializer.Initialize(services, hostContext);

			services.AddTransient<IOutgoingService, TelegramOutgoingService>();
			services.AddTransient<IOutgoingService, DiscordOutgoingService>();

			services.AddTransient<ITransportCommandFactory, TelegramCommandFactory>();
			services.AddTransient<ITransportCommandFactory, DiscordCommandFactory>();
		}

		private static void RegistrateHostedServices(IServiceCollection services)
		{
			services.AddHostedService<TelegramWorker>();
			services.AddHostedService<DiscordWorker>();
		}

		private static void CreateConfigurations(HostBuilderContext hostContext, IServiceCollection services)
		{
			services.AddOptions();
			services.Configure<TelegramOptions>(hostContext.Configuration.GetSection(TelegramOptions.SectionName));
			services.Configure<DiscordOptions>(hostContext.Configuration.GetSection(DiscordOptions.SectionName));
		}
	}
}