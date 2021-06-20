using Counselor.Platform.Interpreter.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Counselor.Platform.Tests.Interpreter.Expressions
{
	[TestClass]
	public class BooleanExpressionResolverTests
	{
		[TestMethod]
		public void BooleanExpressionEqual_True()
		{
			string expression = "a Equal a";
			var resolver = new BooleanExpressionResolver();

			var result = resolver.Resolve(expression);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BooleanExpressionEqual_False()
		{
			string expression = "a Equal b";
			var resolver = new BooleanExpressionResolver();
			
			var result = resolver.Resolve(expression);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void BooleanExpressionNotEqual_True()
		{
			string expression = "a NotEqual b";
			var resolver = new BooleanExpressionResolver();

			var result = resolver.Resolve(expression);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BooleanExpressionNotEqual_False()
		{
			string expression = "a NotEqual a";
			var resolver = new BooleanExpressionResolver();

			var result = resolver.Resolve(expression);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void BooleanExpressionEqualAndEqual_True()
		{
			string expression = "a Equal a And b Equal b";
			var resolver = new BooleanExpressionResolver();

			var result = resolver.Resolve(expression);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BooleanExpressionEqualAndEqual_False()
		{
			string expression = "a Equal a And a Equal b";
			var resolver = new BooleanExpressionResolver();

			var result = resolver.Resolve(expression);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void BooleanExpressionEqualOrEqual_True()
		{
			string expression = "a Equal b Or b Equal b";
			var resolver = new BooleanExpressionResolver();

			var result = resolver.Resolve(expression);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BooleanExpressionEqualOrEqual_False()
		{
			string expression = "a Equal b Or a Equal b";
			var resolver = new BooleanExpressionResolver();

			var result = resolver.Resolve(expression);

			Assert.IsFalse(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BooleanExpression_EmptyExpression()
		{			
			var resolver = new BooleanExpressionResolver();

			resolver.Resolve(string.Empty);			
		}
	}
}
