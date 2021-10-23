using AutoFixture;
using Counselor.Platform.Api.Controllers;
using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Counselor.Platform.Tests.Api.Controllers
{
	[TestClass]
	public class ScriptsControllerTests
	{
		private readonly Fixture _fixture = new Fixture();

		#region GetScript
		[TestMethod]
		public void GetScript_Ok()
		{
			var service = new Mock<IScriptService>();
			var script = _fixture.Create<ScriptDto>();
			service.Setup(x => x.GetScript(It.IsAny<int>())).ReturnsAsync(script);
			var controller = new ScriptsController(service.Object);

			var result = controller.GetScript(script.Id).Result as OkObjectResult;

			Assert.IsNotNull(result);
			var resultScript = result.Value as ScriptDto;
			Assert.IsNotNull(resultScript);
			Assert.AreEqual(script.Id, resultScript.Id);
			Assert.AreEqual(script.Name, resultScript.Name);
			Assert.AreEqual(script.IsActive, resultScript.IsActive);
			Assert.AreEqual(script.Data, resultScript.Data);

			service.Verify(x => x.GetScript(It.Is<int>(v => v == script.Id)), Times.Once);
		}

		[TestMethod]
		public void GetScript_BadRequest()
		{
			var service = new Mock<IScriptService>();
			var controller = new ScriptsController(service.Object);

			var result = controller.GetScript(0).Result as BadRequestResult;

			Assert.IsNotNull(result);
			service.Verify(x => x.GetScript(It.IsAny<int>()), Times.Never);
		}

		[TestMethod]
		public void GetScript_NotFound()
		{
			var service = new Mock<IScriptService>();
			int scriptId = 1;
			service.Setup(x => x.GetScript(It.IsAny<int>())).ReturnsAsync((ScriptDto)null);
			var controller = new ScriptsController(service.Object);

			var result = controller.GetScript(scriptId).Result as NotFoundResult;

			Assert.IsNotNull(result);
			service.Verify(x => x.GetScript(It.Is<int>(v => v == scriptId)), Times.Once);
		}
		#endregion

		#region GetAllScripts
		[TestMethod]
		public void GetAllScripts_Ok()
		{
			var service = new Mock<IScriptService>();
			service.Setup(x => x.GetAllScripts()).ReturnsAsync(_fixture.Build<ScriptHeaderDto>().CreateMany(5));
			var controller = new ScriptsController(service.Object);

			var result = controller.GetAllScripts().Result as OkObjectResult;

			Assert.IsNotNull(result);
			Assert.AreEqual(5, ((IEnumerable<ScriptHeaderDto>)result.Value).Count());
			service.Verify(x => x.GetAllScripts(), Times.Once);
		} 
		#endregion


	}
}
