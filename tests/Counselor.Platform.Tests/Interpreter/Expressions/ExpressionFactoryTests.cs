using AutoFixture;
using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Interpreter.Exceptions;
using Counselor.Platform.Interpreter.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Counselor.Platform.Tests.Interpreter.Expressions
{
	[TestClass]
	public class ExpressionFactoryTests
	{
		private const string Transport = "transport";
		private readonly Fixture _fixture = new Fixture();

		[TestMethod]
		public void ParseExpression_Simple()
		{
			var expressionOperator = _fixture.Create<string>();
			var expressionParameter = _fixture.Create<string>();

			var result = ExpressionFactory.ParseExpression($"[{expressionOperator}] {expressionParameter}");

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.@operator);
			Assert.IsNotNull(result.parameters);
			Assert.AreEqual(expressionOperator, result.@operator);
			Assert.AreEqual(expressionParameter, result.parameters);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidExpressionSyntaxException))]
		public void ParseExpression_InvalidSyntax()
		{
			ExpressionFactory.ParseExpression($"{_fixture.Create<string>()}");
		}

		[TestMethod]
		public void CreateExpression_GetInternalOperator()
		{
			var expressionOperator = "MessageContains";
			var expressionFactory = new ExpressionFactory(new List<ITransportCommandFactory>());

			var result = expressionFactory.CreateExpression($"[{expressionOperator}] text", Transport);

			Assert.IsNotNull(result);
			Assert.AreEqual(expressionOperator, result.Operator);
		}

		[TestMethod]
		public void CreateExpression_GetExternalOperator()
		{
			var internalOperator = "ExternalCommand";
			var externalOperator = "SendMessage";

			var commandFactory = new Mock<ITransportCommandFactory>();
			commandFactory.Setup(x => x.TransportName).Returns(Transport);
			commandFactory.Setup(x => x.CreateCommand(It.IsAny<string>()))
				.Returns(new Mock<ITransportCommand>().Object);
			var expressionFactory = new ExpressionFactory(new List<ITransportCommandFactory> { commandFactory.Object });

			var result = expressionFactory.CreateExpression($"[{internalOperator}] [{externalOperator}] text", Transport);

			Assert.IsNotNull(result);
			Assert.AreEqual(internalOperator, result.Operator);
		}

		[TestMethod]
		[ExpectedException(typeof(NotImplementedException))]
		public void CreateExpression_ExpressionNotImplemented()
		{
			var expressionOperator = _fixture.Create<string>();
			var expressionFactory = new ExpressionFactory(new List<ITransportCommandFactory>());

			expressionFactory.CreateExpression($"[{expressionOperator}] text", Transport);
		}
	}
}
