using Counselor.Platform.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Commands
{
	public abstract class TransportCommandFactory : ITransportCommandFactory
	{
		public abstract string TransportName { get; }
		private readonly Dictionary<string, Type> _commandTypes;

		public TransportCommandFactory()
		{
			foreach (var command in TypeHelpers.GetTypeImplementations<ITransportCommand>())
			{
				if (command.FullName.Contains(TransportName))
					_commandTypes.Add(command.Name, command);
			}
		}

		public virtual ITransportCommand CreateCommand(string transport, string identificator)
		{
			if (!_commandTypes.TryGetValue(identificator, out var type))
				throw new NotImplementedException($"ITransportCommand: {identificator} was not implemented for {TransportName}.");

			return Activator.CreateInstance(type) as ITransportCommand;
		}

		public virtual Task<ITransportCommand> CreateCommandAsync(string identificator)
		{
			throw new NotImplementedException();
		}
	}
}
