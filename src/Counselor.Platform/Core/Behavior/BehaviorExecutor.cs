using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Interpreter;
using Counselor.Platform.Repositories.Interfaces;
using Counselor.Platform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	public class BehaviorExecutor : IBehaviorExecutor
	{
		private readonly IInterpreter _interpreter;
		private readonly ILogger<BehaviorExecutor> _logger;
		private readonly IServiceProvider _serviceProvider;
		private readonly IOutgoingServicePool _outgoingServicePool;
		private readonly IConnectionsRepository _connectionsRepository;
		private readonly IDialogsRepository _dialogsRepository;
		private readonly ServiceOptions _options;
		private readonly SemaphoreSlim _executorSemaphore = new SemaphoreSlim(1, 1);
		private readonly Timer _timer;
		private Behavior _behavior;

		private readonly Dictionary<string, BehaviorContext> _runningDialogs = new Dictionary<string, BehaviorContext>();
		private const string DefaultBehaviorInstructionNext = "Next";
		private const string DefaultBehaviorInstructionNone = "None";

		public BehaviorExecutor(
			IInterpreter interpreter,
			ILogger<BehaviorExecutor> logger,
			IServiceProvider serviceProvider,
			IOutgoingServicePool outgoingServicePool,
			IConnectionsRepository connectionsRepository,
			IDialogsRepository dialogsRepository,
			IOptions<ServiceOptions> options
			)
		{
			_interpreter = interpreter;
			_logger = logger;
			_serviceProvider = serviceProvider;
			_outgoingServicePool = outgoingServicePool;
			_connectionsRepository = connectionsRepository;
			_dialogsRepository = dialogsRepository;
			_options = options.Value;

			_timer = new Timer(ClearExpiredDialogs, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(_options.DialogTTLMs * 2));
		}

		public void Initialize(int scriptId, IPlatformDatabase database)
		{
			try
			{
				var script = database.Scripts.First(x => x.Id == scriptId);

				if (!string.IsNullOrEmpty(script?.Name))
				{
					var deserializer = new YamlDotNet.Serialization.Deserializer();
					using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(script.Instruction)))
					using (var reader = new StreamReader(stream))
					{
						var behavior = deserializer.Deserialize<Behavior>(reader.ReadToEnd());

						if (BehaviorValidator.IsValid(behavior))
						{
							_behavior = behavior;
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, $"Cannot build behavior. ScriptId: {scriptId}.");
				throw;
			}
		}

		public async Task RunBehaviorLogicAsync(string connectionId, string username, string payload, Services.ServiceContext context)
		{
			var behaviorContext = await CreateBehaviorContextAsync(connectionId, username, payload, context);

			var availableSteps = behaviorContext.Iterator.Current();
			BehaviorStep currentStep = null;

			foreach (var step in availableSteps)
			{
				if (await CheckStepCondition(behaviorContext, step))
				{
					currentStep = step;
					break;
				}
			}

			if (currentStep != null)
			{
				behaviorContext.Iterator.Next(currentStep?.Id);
				await RunStepAsync(behaviorContext, _outgoingServicePool.Resolve(context.TransportName), currentStep);
			}

			if (!behaviorContext.Iterator.Current().Any())
			{
				behaviorContext.Iterator.Reset();
			}
		}

		private async Task<BehaviorContext> CreateBehaviorContextAsync(string connectionId, string username, string payload, Services.ServiceContext serviceContext)
		{
			try
			{
				await _executorSemaphore.WaitAsync();

				if (!_runningDialogs.TryGetValue(connectionId, out var context))
				{
					_logger.LogTrace($"Behavior context was created for dialog. BotId: {serviceContext.BotId}. ConnectionId: {connectionId}.");

					var database = _serviceProvider.GetRequiredService<IPlatformDatabase>();
					var client = await FindOrCreateUserAsync(connectionId, username, database, serviceContext);

					context = new BehaviorContext
					{
						ConnectionId = connectionId,
						Iterator = _behavior.Iterator,
						ServiceContext = serviceContext,
						Database = database,
						Client = client
					};

					_runningDialogs.Add(connectionId, context);
				}

				context.Dialog = await _dialogsRepository.CreateOrUpdateDialogAsync(context.Database, context.Client, payload, MessageDirection.In, serviceContext.BotId);
				context.LastUsedOn = DateTime.Now;

				return context;
			}
			finally
			{
				_executorSemaphore.Release();
			}
		}

		private async Task<bool> CheckStepCondition(BehaviorContext context, BehaviorStep step)
		{
			if (!string.IsNullOrEmpty(step.Condition?.Instruction))
			{
				if (step.Condition.Instruction.Equals(DefaultBehaviorInstructionNone)) return true;
				if (step.Condition.Instruction.Equals(DefaultBehaviorInstructionNext)) return false;
				return (await _interpreter.Interpret(step.Condition, context.Dialog, context.Database, context.ServiceContext.TransportName)).GetTypedResult<bool>();
			}

			return false;
		}

		private async Task RunStepAsync(BehaviorContext context, IOutgoingService outgoingService, BehaviorStep step)
		{
			_logger.LogDebug($"Step: {step.Id} is running for client: {context.Client.Id} within bot: {context.ServiceContext.BotId}.");

			try
			{
				if (step.Command != null)
				{
					if (step.Command.Type == Enums.BehaviorInstructionType.Instruction //todo: не учитываются кейсы отсутвия инструкции и системной команды в инструкции
						&& !string.IsNullOrEmpty(step.Command?.Instruction)
						&& !step.Command.Instruction.Equals(DefaultBehaviorInstructionNone))
					{
						await HandleInterpretationResult(await _interpreter.Interpret(step.Command, context.Dialog, context.Database, context.ServiceContext.TransportName), context);
					}
				}

				if (!string.IsNullOrEmpty(step.Response))
				{
					var response = await _interpreter.InsertEntityParameters(step.Response, context.Dialog, context.Database);
					await outgoingService.SendAsync(response, context.Client.Id);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Behavior step failed. StepId: {step.Id}. BotId: {context.ServiceContext.BotId}. ClientId: {context.Client.Id}.");
				throw;
			}
		}

		private async Task HandleInterpretationResult(InterpretationResult result, BehaviorContext context)
		{
			if (result.State == InterpretationResultState.Failed)
			{
				_logger.LogError($"Interpretation step failed. BotId: {context.ServiceContext.BotId}");
				await _outgoingServicePool.Resolve(context.ServiceContext.TransportName).SendAsync("Невозможно выполнить команду. Обратитесь к системному администратору.", context.Client.Id); //todo: обработка ошибки интерпретации
			}


			if (result.ResultType == Interpreter.Expressions.ExpressionResultType.TransportCommand)
			{
				result.Command.ConnectionId = context.ConnectionId;
				await _outgoingServicePool.Resolve(context.ServiceContext.TransportName).SendAsync(result.Command);
			}
		}

		private async Task<User> FindOrCreateUserAsync(string connectionId, string username, IPlatformDatabase database, Services.ServiceContext serviceContext)
		{
			var client =
				(await database.UserTransports
					.Include(x => x.User)
					.FirstOrDefaultAsync(x => x.Id == serviceContext.TransportId && x.TransportUserId.Equals(connectionId)))?.User;

			if (client is null)
			{
				client = new User
				{
					Username = username
				};

				database.UserTransports.Add(
					new UserTransport
					{
						Transport = await database.Transports.FirstAsync(x => x.Id == serviceContext.TransportId),
						User = client,
						TransportUserId = connectionId
					});

				_logger.LogInformation($"New user just created. Username: {username}. Transport: {serviceContext.TransportName}. BotId: {serviceContext.BotId}.");
			}

			client.LastActivity = DateTime.Now;
			await database.SaveChangesAsync();

			_connectionsRepository.AddConnection(client.Id, serviceContext.TransportName, connectionId);

			return client;
		}

		private void ClearExpiredDialogs(object state)
		{
			foreach (var key in _runningDialogs.Keys)
			{
				if (_runningDialogs[key].LastUsedOn.AddMilliseconds(_options.DialogTTLMs) < DateTime.Now)
				{
					_runningDialogs.Remove(key);
				}
			}
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
