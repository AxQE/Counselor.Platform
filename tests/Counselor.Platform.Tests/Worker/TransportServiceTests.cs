using AutoFixture;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Data.Entities.Enums;
using Counselor.Platform.Data.Options;
using Counselor.Platform.Worker.Transport;
using Counselor.Platform.Worker.Transport.Telegram;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Tests.Worker
{
	[TestClass]
	public class TransportServiceTests
	{
		private const string TelegramSystemName = "Telegram";
		private const string TelegramConfig = "{\r\n\t\"SendErrorReport\": true,\r\n\t\"Token\": \"12343545:aaaaaaaaaaaaaaaaa\",\r\n\t\"DialogName\": \"Sample\",\r\n\t\"IsEnabled\": true\r\n}";
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

			var bot = _fixture.Build<Bot>()
				.With(x => x.BotState, BotState.Pending)
				.With(x => x.Configuration, TelegramConfig)
				.With(x => x.Transport, new Transport { Name = TelegramSystemName, IsActive = true })
				.With(x => x.Script, new Script { IsActive = true, Data = _fixture.Create<string>() })
				.Create();

			_databaseMock.Setup(x => x.Bots).ReturnsDbSet(new List<Bot> { bot });

			var service = CreateService();

			service.StartBotsAsync().Wait();

			var runningBotDataObject = ((IEnumerable<object>)service.GetType().GetField("_runningBots", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(service))
				.Single();

			var result = (Bot)runningBotDataObject.GetType().GetProperty("Bot").GetValue(runningBotDataObject);

			Assert.IsNotNull(result);
			Assert.AreEqual(bot.Id, result.Id);
			Assert.AreEqual(BotState.Started, result.BotState);
		}

		private TransportService CreateService()
		{
			var options = new TransportServiceOptions
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

			return new TransportService(_loggerFactory.CreateLogger<TransportService>(), _databaseMock.Object, _serviceProviderMock.Object, Options.Create(options));
		}
	}
}
