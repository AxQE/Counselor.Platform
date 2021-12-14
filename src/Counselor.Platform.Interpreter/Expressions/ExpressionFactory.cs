using Counselor.Platform.Interpreter.Commands;
using Counselor.Platform.Interpreter.Exceptions;
using Counselor.Platform.Interpreter.Expressions.Operators;
using Counselor.Platform.Utils;
using System;
using System.Collections.Generic;

namespace Counselor.Platform.Interpreter.Expressions
{
	public class ExpressionFactory : IExpressionFactory
	{
		private readonly Dictionary<string, Type> _expressionTypes = new Dictionary<string, Type>();
		private readonly Dictionary<string, ITransportCommandFactory> _externalCommandFactories = new Dictionary<string, ITransportCommandFactory>();

		public ExpressionFactory(IEnumerable<ITransportCommandFactory> commandFactories)
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

		public IExpression CreateExpression(string instruction, string transport)
		{
			var expression = ParseExpression(instruction);

			if (!_expressionTypes.TryGetValue(expression.@operator, out var type))
				throw new NotImplementedException($"Interpreter operator named {expression.@operator} not implemented.");

			if (expression.@operator.Equals(nameof(ExternalCommand)))
				return new ExternalCommand(_externalCommandFactories[transport], expression.parameters);

			return Activator.CreateInstance(type, expression.parameters) as IExpression;
		}

		public static (string @operator, string parameters) ParseExpression(string instruction)
		{
			var operatorOpen = instruction?.IndexOf('[') ?? -1;
			var operatorClose = instruction?.IndexOf(']') ?? -1;

			if (operatorOpen == -1 || operatorClose == -1)
				throw new InvalidExpressionSyntaxException("Instruction does not contain expression operator.");

			var @operator = instruction.Substring(1, operatorClose - 1);
			var parameters = instruction.Substring(operatorClose + 1).Trim();

			return (@operator, parameters);
		}
	}
}
