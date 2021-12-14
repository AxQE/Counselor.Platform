using Counselor.Platform.Api.Controllers;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace Counselor.Platform.Tests.Api.Controllers
{
	[TestClass]
	public class TransportsControllerTests
	{
		[TestMethod]
		public void GetAllTransports_Simple()
		{
			var service = new Mock<ITransportService>();
			service.Setup(x => x.GetAllTransports(It.IsAny<CancellationToken>()));
			var controller = new TransportsController(service.Object);

			var result = controller.GetAllTransports(CancellationToken.None).Result as OkObjectResult;

			Assert.IsNotNull(result);
			service.Verify(x => x.GetAllTransports(It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}
