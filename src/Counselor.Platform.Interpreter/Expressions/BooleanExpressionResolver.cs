using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Counselor.Platform.Tests")]
namespace Counselor.Platform.Interpreter.Expressions
{
	public class BooleanExpressionResolver
	{
		private readonly Stack<string> _expressionStack = new Stack<string>();
		private readonly Queue<string> _expressionQueue = new Queue<string>();

		private static readonly Regex _logicalOperationRegex = new Regex("And|Equal|NotEqual|Or");
		private static readonly Regex _highPriorityOperationsRegex = new Regex("Equal|NotEqual");
		private static readonly Regex _lowPriorityOperationsRegex = new Regex("And|Or");

		private static readonly Dictionary<string, Func<string, string, bool>> _expressionOperations =
			new Dictionary<string, Func<string, string, bool>>
		{
			{"Equal", (left ,right) => left.Equals(right, StringComparison.OrdinalIgnoreCase) },
			{"NotEqual", (left ,right) => !left.Equals(right, StringComparison.OrdinalIgnoreCase) },
			{"And", (left ,right) => left.Equals("True", StringComparison.OrdinalIgnoreCase) && right.Equals("True", StringComparison.OrdinalIgnoreCase)},
			{"Or", (left ,right) => left.Equals("True", StringComparison.OrdinalIgnoreCase) || right.Equals("True", StringComparison.OrdinalIgnoreCase) }
		};


		/*
		enum Token
		{
			And,            // And
			Equal,          // Equal
			Not,            // Not
			NotEqual,       // NotEqual
			Or,             // Or
			OpenBracket,	// (
			CloseBracket,	// )
			Value           // [a-zA-Z0-9]*
		}
		*/

		public bool Resolve(string expression)
		{
			if (string.IsNullOrEmpty(expression))
				throw new ArgumentNullException(nameof(expression), "Boolean expression cannot be null or empty.");
			
			ConvertToPostfixExpression(expression);
			return CalculateExpression();
		}

		private void ConvertToPostfixExpression(string expression)
		{
			var operands = expression.Split(' ');

			foreach (var operand in operands)
			{
				if (operand.Equals("("))
				{
					_expressionStack.Push(operand);
				}
				else if (operand.Equals(")"))
				{
					if (operands.Contains("(")) MoveFromStackToQueue();
				}
				else if (!_logicalOperationRegex.IsMatch(operand))
				{
					_expressionQueue.Enqueue(operand);
				}
				else if (_lowPriorityOperationsRegex.IsMatch(operand))
				{
					if (!_expressionStack.Any()
						|| _expressionStack.TryPeek(out var i) && i == "(")
					{
						_expressionStack.Push(operand);
					}
					else if (_expressionStack.TryPeek(out var j)
						&& _highPriorityOperationsRegex.IsMatch(j))
					{
						MoveFromStackToQueue();
						_expressionStack.Push(operand);
					}
					else
					{
						_expressionQueue.Enqueue(_expressionStack.Pop());
						_expressionStack.Push(operand);
					}
				}
				else if (_highPriorityOperationsRegex.IsMatch(operand))
				{
					if (!_expressionStack.Any()
						&& _expressionStack.TryPeek(out var i)
						&& _highPriorityOperationsRegex.IsMatch(i))
					{
						MoveFromStackToQueue();
					}

					_expressionStack.Push(operand);
				}				
			}

			if (_expressionStack.Any())
			{
				while (_expressionStack.Any())
				{
					var i = _expressionStack.Pop();
					if (i != "(") _expressionQueue.Enqueue(i);
				}
			}
		}

		private void MoveFromStackToQueue()
		{
			while (_expressionStack.Any())
			{
				var i = _expressionStack.Pop();
				if (i != "(") _expressionQueue.Enqueue(i);
				else break;
			}
		}

		//todo: нужно заменить алгоритм на строках на построение деревий выражений. Это позволит в булево выражение встраивать кастомные команды
		private bool CalculateExpression()
		{
			while (_expressionQueue.Any() && _expressionQueue.TryDequeue(out var i))
			{
				if (!_logicalOperationRegex.IsMatch(i))
				{
					_expressionStack.Push(i);
				}
				else
				{
					var result = _expressionOperations[i](_expressionStack.Pop(), _expressionStack.Pop()).ToString();
					_expressionStack.Push(result);
				}
			}

			return bool.Parse(_expressionStack.Pop());
		}
	}
}