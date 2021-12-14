using AutoFixture;
using Counselor.Platform.Core.Behavior;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Worker.Transport;
using Counselor.Platform.Worker.Transport.Telegram;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Counselor.Platform.Tests.Worker
{
	[TestClass]
	public class TransportServiceTests
	{
		private const string TelegramSystemName = "Telegram";
		private const string TelegramConfig = "{\"SendErrorReport\": true,\"Token\": \"12343545:aaaaaaaaaaaaaaaaa\",\"DialogName\": \"Sample\",\"IsEnabled\": true\n}";
		private readonly Fixture _fixture = new Fixture();
		private readonly LoggerFactory _loggerFactory = new LoggerFactory();
		private Mock<IPlatformDatabase> _databaseMock;
		private Mock<IServiceProvider> _serviceProviderMock;

		[TestInitialize]
		public void Init()
		{
			_databaseMock = new Mock<IPlatformDatabase>();
			_serviceProviderMock = new Mock<IServiceProvider>();
		}

		[TestMethod]
		public void CreateBot_Simple()
		{
			_serviceProviderMock.Setup(x => x.GetService(typeof(ILogger<TelegramService>)))
				.Returns(_loggerFactory.CreateLogger<TelegramService>());

			_serviceProviderMock.Setup(x => x.GetService(typeof(IBehaviorExecutor)))
				.Returns(
				new BehaviorExecutor(
					null,
					null,
					null,
					null,
					null,
					null,
					Options.Create(new ServiceOptions())));

			_serviceProviderMock.Setup(x => x.GetService(typeof(IPlatformDatabase)))
				.Returns(_databaseMock.Object);

			var bot = _fixture.Build<Bot>()
				.With(x => x.BotState, BotState.Pending)
				.With(x => x.Configuration, TelegramConfig)
				.With(x => x.Transport, new Transport { Name = TelegramSystemName, IsActive = true })
				.With(x => x.Script, new Script { Instruction = _fixture.Create<string>() })
				.Create();

			_databaseMock.Setup(x => x.Bots).ReturnsDbSet(new List<Bot> { bot });
			_databaseMock.Setup(x => x.Scripts).ReturnsDbSet(new List<Script> { new Script { Instruction = string.Empty } });

			var service = CreateService();

			service.StartBotsAsync(_databaseMock.Object).Wait();
			service.StartBotsAsync(_databaseMock.Object).Wait();

			var runningBotDataObject = ((IEnumerable<object>)service.GetType().GetField("_runningBots", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(service))
				.Single();

			var result = (Bot)runningBotDataObject.GetType().GetProperty("Bot").GetValue(runningBotDataObject);

			Assert.IsNotNull(result);
			Assert.AreEqual(bot.Id, result.Id);
			Assert.AreEqual(BotState.Started, result.BotState);
		}

		[TestMethod]
		public void StopBots_Simple()
		{
			_serviceProviderMock.Setup(x => x.GetService(typeof(ILogger<TelegramService>)))
				.Returns(_loggerFactory.CreateLogger<TelegramService>());

			var bot = _fixture.Build<Bot>()
				.With(x => x.BotState, BotState.Pending)
				.With(x => x.Configuration, TelegramConfig)
				.With(x => x.Transport, new Transport { Name = TelegramSystemName, IsActive = true })
				.With(x => x.Script, new Script { Instruction = _fixture.Create<string>() })
				.Create();

			_databaseMock.Setup(x => x.Bots).ReturnsDbSet(new List<Bot> { bot });

			var service = CreateService();

			service.StartBotsAsync(_databaseMock.Object).Wait();

			bot.BotState = BotState.Stopped;
			bot.ModifiedOn = DateTime.Now;

			service.StopBotsAsync(_databaseMock.Object).Wait();

			var runningBotDataObject = ((IEnumerable<object>)service.GetType().GetField("_runningBots", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(service))
				.FirstOrDefault();

			Assert.IsNull(runningBotDataObject);
		}

		private TransportService CreateService()
		{
			var options = new ServiceOptions
			{
				ServiceIntervalMs = 100000,
				Transports = new List<TransportOptions>
				{
					new TransportOptions
					{
						IsEnabled = true,
						SendErrorReport = false,
						SystemName = TelegramSystemName
					}
				}
			};

			return new TransportService(_loggerFactory.CreateLogger<TransportService>(), _serviceProviderMock.Object, Options.Create(options));
		}
	}
}
