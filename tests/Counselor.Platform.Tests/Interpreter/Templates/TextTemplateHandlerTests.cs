using AutoFixture;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Counselor.Platform.Tests.Interpreter.Templates
{
	[TestClass]
	public class TextTemplateHandlerTests
	{
		private readonly Fixture _fixture = new Fixture();

		[TestMethod]
		public void InsertEntityParameters_Simple()
		{
			var template = "{user:username}";
			var username = _fixture.Create<string>();

			var dialog = _fixture.Build<Dialog>()
				.With(x => x.Client, new User { Username = username })
				.Create();

			var result = TextTemplateHandler.InsertEntityParameters(template, dialog, null).Result;

			Assert.AreEqual(username, result);
		}

		[TestMethod]
		public void InsertEntityParameters_Null()
		{
			const string template = "{user:username}";

			var dialog = _fixture.Build<Dialog>()
				.With(x => x.Client, new User { Username = null })
				.Create();

			var result = TextTemplateHandler.InsertEntityParameters(template, dialog, null).Result;

			Assert.AreEqual("null", result);
		}
	}
}
