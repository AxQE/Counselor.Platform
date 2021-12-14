using AutoFixture;
using Counselor.Platform.Api.Services;
using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Counselor.Platform.Tests.Api.Services
{
	[TestClass]
	public class TransportServiceTests
	{
		private readonly Fixture _fixture = new Fixture();

		[TestMethod]
		public void GetAllTransports_Simple()
		{
			var db = new Mock<IPlatformDatabase>();
			var logger = new Mock<ILogger<TransportService>>();
			var transport = _fixture.Create<Transport>();

			var data = new List<Transport>
			{
				transport
			};
			db.Setup(x => x.Transports).ReturnsDbSet(data);

			var service = new TransportService(db.Object, logger.Object);

			var result = service.GetAllTransports(CancellationToken.None).Result;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
			Assert.AreEqual(transport.Name, result.First().Name);
		}
	}
}
