using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Commands;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Expressions.Operators
{
	class ExternalCommand : IExpression
	{
		private readonly ITransportCommandFactory _commandFactory;
		private readonly string _parameters;

		public string Operator => nameof(ExternalCommand);

		public ExternalCommand(ITransportCommandFactory commandFactory, string parameters)
		{
			_commandFactory = commandFactory;
			_parameters = parameters;
		}

		public async Task<InterpretationResult> InterpretAsync(IPlatformDatabase database, Dialog dialog)
		{
			var expression = ExpressionParser.ParseExpression(_parameters);

			var command = _commandFactory.CreateCommand(expression.@operator);
			command.Parameter = expression.parameters;

			return new InterpretationResult
			{
				Command = command,
				ResultType = ExpressionResultType.TransportCommand,
				State = InterpretationResultState.Completed
			};
		}
	}
}
