using Counselor.Platform.Api.Services;
using Counselor.Platform.Api.Services.Interfaces;
using Counselor.Platform.Data.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Counselor.Platform.Api
{
	public class Startup
	{
		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddYamlFile("platformsettings.yaml", optional: false, reloadOnChange: true)
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

			services.AddControllers(c =>
			{
				c.RespectBrowserAcceptHeader = true;
				c.ReturnHttpNotAcceptable = true;
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Counselor.Platform.Api", Version = "v1" });
			});

			services.AddScoped<IUserService, UserService>();
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

			app.UseRouting();
			

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			//ConfigureExceptionHandler(app, logger);			
		}

		private static void ConfigureExceptionHandler(IApplicationBuilder app, ILogger<Startup> logger)
		{
			app.UseExceptionHandler(error =>
			{
				error.Run(async context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					context.Response.ContentType = "application/json";

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

					if (contextFeature != null)
					{
						logger.LogError($"Unhadled error during request: {contextFeature.Error}");
						//await context.Response.WriteAsync(); todo: error response
					}
				});
			});
		}
	}
}
