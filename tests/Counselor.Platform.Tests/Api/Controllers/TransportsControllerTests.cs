using Counselor.Platform.Api.Controllers;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Counselor.Platform.Tests.Api.Controllers
{
	[TestClass]
	public class TransportsControllerTests
	{
		[TestMethod]
		public void GetAllTransports_Simple()
		{
			var service = new Mock<ITransportService>();
			service.Setup(x => x.GetAllTransports());
			var controller = new TransportsController(service.Object);

			var result = controller.GetAllTransports().Result as OkObjectResult;

			Assert.IsNotNull(result);
			service.Verify(x => x.GetAllTransports(), Times.Once);
		}
	}
}
