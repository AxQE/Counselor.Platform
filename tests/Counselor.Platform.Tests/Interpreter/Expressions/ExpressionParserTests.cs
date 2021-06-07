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
	public class ExpressionParserTests
	{
		private const string Transport = "transport";
		private readonly Fixture _fixture = new Fixture();

		[TestMethod]
		public void ParseExpression_Simple()
		{
			var expressionOperator = _fixture.Create<string>();
			var expressionParameter = _fixture.Create<string>();
			
			var result = ExpressionParser.ParseExpression($"[{expressionOperator}] {expressionParameter}");

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.@operator);
			Assert.IsNotNull(result.parameters);
			Assert.AreEqual(expressionOperator, result.@operator);
			Assert.AreEqual(expressionParameter, result.parameters);
		}

		[TestMethod]
		public void ParseExpression_GetInternalOperator()
		{
			var expressionOperator = "MessageContains";
			var expressionParser = new ExpressionParser(new List<ITransportCommandFactory>());

			var result = expressionParser.Parse($"[{expressionOperator}] text", Transport);

			Assert.IsNotNull(result);
			Assert.AreEqual(expressionOperator, result.Operator);
		}

		[TestMethod]
		public void ParseExpression_GetExternalOperator()
		{
			var internalOperator = "ExternalCommand";
			var externalOperator = "SendMessage";

			var commandFactory = new Mock<ITransportCommandFactory>();
			commandFactory.Setup(x => x.TransportName).Returns(Transport);
			commandFactory.Setup(x => x.CreateCommand(It.IsAny<string>()))
				.Returns(new Mock<ITransportCommand>().Object);
			var expressionParser = new ExpressionParser(new List<ITransportCommandFactory> { commandFactory.Object });

			var result = expressionParser.Parse($"[{internalOperator}] [{externalOperator}] text", Transport);

			Assert.IsNotNull(result);
			Assert.AreEqual(internalOperator, result.Operator);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidExpressionSyntaxException))]
		public void ParseExpression_ExpressionNameDoesNotExist()
		{
			var expressionOperator = "SomeOperator";
			var expressionParameter = "Parameters";

			ExpressionParser.ParseExpression($"{expressionOperator} {expressionParameter}");
		}

		[TestMethod]
		[ExpectedException(typeof(NotImplementedException))]
		public void ParseExpression_ExpressionNotImplemented()
		{
			var expressionOperator = _fixture.Create<string>();
			var expressionParser = new ExpressionParser(new List<ITransportCommandFactory>());

			expressionParser.Parse($"[{expressionOperator}] text", Transport);
		}
	}
}
