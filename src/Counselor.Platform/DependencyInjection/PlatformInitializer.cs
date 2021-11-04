using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.DependencyInjection;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Interpreter;
using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Interpreter.Expressions;
using Counselor.Platform.Interpreter.Templates;
using Counselor.Platform.Repositories;
using Counselor.Platform.Repositories.Interfaces;
using Counselor.Platform.Services;
using Counselor.Platform.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

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
			services.AddTransient<IBehaviorExecutor, BehaviorExecutor>();
			#endregion

			#region interpreter
			services.AddTransient<IInterpreter, InterpreterRuntime>();
			services.AddSingleton<IExpressionFactory, ExpressionFactory>();
			#endregion

			#region repositories
			services.AddSingleton<IConnectionsRepository, ConnectionsRepository>();
			services.AddSingleton<IDialogsRepository, DialogsRepository>();
			#endregion

			DatabaseDI.ConfigureDatabase(services, hostContext.Configuration);
		}

		private static void CreateConfigurations(IServiceCollection services, HostBuilderContext hostContext)
		{
			services.AddOptions();
			services.Configure<CacheOptions>(hostContext.Configuration.GetSection(CacheOptions.SectionName));
			services.Configure<DatabaseOptions>(hostContext.Configuration.GetSection(DatabaseOptions.SectionName));
			services.Configure<PlatformOptions>(hostContext.Configuration.GetSection(PlatformOptions.SectionName));
			services.Configure<ServiceOptions>(hostContext.Configuration.GetSection(ServiceOptions.SectionName));
		}		
	}
}