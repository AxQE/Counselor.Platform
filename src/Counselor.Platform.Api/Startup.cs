using Counselor.Platform.Api.Helpers;
using Counselor.Platform.Api.Middleware;
using Counselor.Platform.Api.Services;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Counselor.Platform.Api
{
	public class Startup
	{
		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddYamlFile("apisettings.yaml", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.AddUserSecrets<Startup>();

			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			DatabaseDI.ConfigureDatabase(services, Configuration, ServiceLifetime.Scoped);
			services.AddMemoryCache();

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder =>
					{
						builder.AllowAnyOrigin();
						builder.AllowAnyHeader();
						builder.AllowAnyMethod();
					});
			});

			services.AddControllers(c =>
			{
				c.RespectBrowserAcceptHeader = true;
				c.ReturnHttpNotAcceptable = true;
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Counselor.Platform.Api", Version = "v1" });
			});

			services.AddResponseCaching();

			services.AddAuthentication("BasicAuthentication")
				.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ITransportService, TransportService>();
			services.AddScoped<IEntitiesService, EntitiesService>();
			services.AddScoped<IDialogService, DialogService>();
			services.AddScoped<IScriptService, ScriptService>();
			services.AddScoped<IBotService, BotService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Counselor.Platform.Api v1"));
			}

			app.UseHttpsRedirection();

			app.UseMiddleware<LoggingMiddleware>();

			app.UseCors();

			app.UseResponseCaching();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
