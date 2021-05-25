using Counselor.Platform.Interpreter.Exceptions;
using Counselor.Platform.Interpreter.Expressions;
using System;

namespace Counselor.Platform.Interpreter
{
	static class ExpressionParser
	{
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
			switch (@operator)
			{
				case "MessageContains":
					return new MessageConstains(parameters);

				case "HistoryContains":
					return new HistoryConstains(parameters);

				default:
					throw new NotImplementedException($"Interpretor operator named {@operator} not implemented.");
			}			
		}
	}
}
