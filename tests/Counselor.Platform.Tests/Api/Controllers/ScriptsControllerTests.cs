using AutoFixture;
using Counselor.Platform.Api.Controllers;
using Counselor.Platform.Api.Entities.Dto;
using Counselor.Platform.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Counselor.Platform.Tests.Api.Controllers
{
	[TestClass]
	public class ScriptsControllerTests
	{
		private readonly Fixture _fixture = new Fixture();

		private ScriptsController CreateController(IScriptService service, int userId = 1, string username = "user")
		{			
			var httpContext = new Mock<HttpContext>();			

			httpContext.SetupGet(x => x.User)
				.Returns
				(
					new ClaimsPrincipal
					(
						new ClaimsIdentity
						(
							new List<Claim>
							{
								new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
								new Claim(ClaimTypes.Name, username)
							}
						)
					)
				);

			var controller = new ScriptsController(service)
			{
				ControllerContext = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()))
			};

			return controller;
		}

		#region GetScript
		[TestMethod]
		public void GetScript_Ok()
		{
			var service = new Mock<IScriptService>();
			var script = _fixture.Create<ScriptDto>();
			int userId = _fixture.Create<int>();
			service.Setup(x => x.GetScript(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(script);
			var controller = CreateController(service.Object, userId);

			var result = controller.GetScript(script.Id).Result as OkObjectResult;

			Assert.IsNotNull(result);
			var resultScript = result.Value as ScriptDto;
			Assert.IsNotNull(resultScript);
			Assert.AreEqual(script.Id, resultScript.Id);
			Assert.AreEqual(script.Name, resultScript.Name);
			Assert.AreEqual(script.IsActive, resultScript.IsActive);
			Assert.AreEqual(script.Data, resultScript.Data);

			service.Verify(x => x.GetScript(It.Is<int>(v => v == script.Id), It.Is<int>(v => v == userId)), Times.Once);
		}

		[TestMethod]
		public void GetScript_BadRequest()
		{
			var service = new Mock<IScriptService>();
			var controller = CreateController(service.Object);

			var result = controller.GetScript(0).Result as BadRequestResult;

			Assert.IsNotNull(result);
			service.Verify(x => x.GetScript(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
		}

		[TestMethod]
		public void GetScript_NotFound()
		{
			var service = new Mock<IScriptService>();
			int scriptId = _fixture.Create<int>();
			int userId = _fixture.Create<int>();
			service.Setup(x => x.GetScript(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((ScriptDto)null);
			var controller = CreateController(service.Object, userId);

			var result = controller.GetScript(scriptId).Result as NotFoundResult;

			Assert.IsNotNull(result);
			service.Verify(x => x.GetScript(It.Is<int>(v => v == scriptId), It.Is<int>(v => v == userId)), Times.Once);
		}
		#endregion

		#region GetAllScripts
		[TestMethod]
		public void GetAllScripts_Ok()
		{
			var service = new Mock<IScriptService>();
			int userId = _fixture.Create<int>();
			service.Setup(x => x.GetAllScripts(It.IsAny<int>())).ReturnsAsync(_fixture.Build<ScriptHeaderDto>().CreateMany(5));
			var controller = CreateController(service.Object, userId);

			var result = controller.GetAllScripts().Result as OkObjectResult;

			Assert.IsNotNull(result);
			Assert.AreEqual(5, ((IEnumerable<ScriptHeaderDto>)result.Value).Count());
			service.Verify(x => x.GetAllScripts(It.Is<int>(v => v == userId)), Times.Once);
		}
		#endregion


	}
}
