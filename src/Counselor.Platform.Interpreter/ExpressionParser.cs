using Counselor.Platform.Interpreter.Exceptions;
using Counselor.Platform.Interpreter.Expressions;
using Counselor.Platform.Utils;
using System;
using System.Collections.Generic;

namespace Counselor.Platform.Interpreter
{
	static class ExpressionParser
	{
		private static readonly Dictionary<string, Type> _expressionTypes = new Dictionary<string, Type>();

		static ExpressionParser()
		{
			foreach (var type in TypeHelpers.GetTypeImplementations<IExpression>())
			{
				_expressionTypes.Add(type.Name, type);
			}			
		}

		public static IExpression Parse(IInstruction instruction)
		{			
			var operatorOpen = instruction?.Instruction?.IndexOf('[') ?? -1;
			var operatorClose = instruction?.Instruction?.IndexOf(']') ?? -1;

			if (operatorOpen == -1 || operatorClose == -1)
				throw new InvalidExpressionSyntaxException();

			var @operator = instruction.Instruction.Substring(1, operatorClose - 1);
			var parameters = instruction.Instruction.Substring(operatorClose + 1).Trim();

			return ExpressionFactory(@operator, parameters);
		}

		private static IExpression ExpressionFactory(string @operator, string parameters)
		{
			if (!_expressionTypes.TryGetValue(@operator, out var type))
				throw new NotImplementedException($"Interpreter operator named {@operator} not implemented.");

			return Activator.CreateInstance(type, parameters) as IExpression;
		}
	}
}
