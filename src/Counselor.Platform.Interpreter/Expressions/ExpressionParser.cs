using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Interpreter.Exceptions;
using Counselor.Platform.Interpreter.Expressions.Operators;
using Counselor.Platform.Utils;
using System;
using System.Collections.Generic;

namespace Counselor.Platform.Interpreter.Expressions
{
	public class ExpressionParser : IExpressionParser
	{
		private static readonly Dictionary<string, Type> _expressionTypes = new Dictionary<string, Type>();
		private static readonly Dictionary<string, ITransportCommandFactory> _externalCommandFactories = new Dictionary<string, ITransportCommandFactory>();

		public ExpressionParser(IEnumerable<ITransportCommandFactory> commandFactories)
		{
			foreach (var type in TypeHelpers.GetTypeImplementations<IExpression>())
			{
				_expressionTypes.Add(type.Name, type);
			}

			foreach (var factory in commandFactories)
			{
				_externalCommandFactories.Add(factory.TransportName, factory);
			}
		}

		public IExpression Parse(string instruction, string transport)
		{
			var expression = ParseExpressionName(instruction);
			return ExpressionFactory(expression.@operator, expression.parameters, transport);
		}

		public static (string @operator, string parameters) ParseExpressionName(string instruction)
		{
			var operatorOpen = instruction?.IndexOf('[') ?? -1;
			var operatorClose = instruction?.IndexOf(']') ?? -1;

			if (operatorOpen == -1 || operatorClose == -1)
				throw new InvalidExpressionSyntaxException();

			var @operator = instruction.Substring(1, operatorClose - 1);
			var parameters = instruction.Substring(operatorClose + 1).Trim();

			return (@operator, parameters);
		}

		private static IExpression ExpressionFactory(string @operator, string parameters, string transport)
		{
			if (!_expressionTypes.TryGetValue(@operator, out var type))
				throw new NotImplementedException($"Interpreter operator named {@operator} not implemented.");

			if (@operator.Equals(nameof(ExternalCommand)))
				return new ExternalCommand(_externalCommandFactories[transport], parameters);

			return Activator.CreateInstance(type, parameters) as IExpression;
		}
	}
}
