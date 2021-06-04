using Counselor.Platform.Utils;
using System;
using System.Collections.Generic;

namespace Counselor.Platform.Interpreter.Commands
{
	public abstract class TransportCommandFactory : ITransportCommandFactory
	{
		public abstract string TransportName { get; }
		private readonly Dictionary<string, Type> _commandTypes = new Dictionary<string, Type>();

		public TransportCommandFactory()
		{
			foreach (var command in TypeHelpers.GetTypeImplementations<ITransportCommand>())
			{
				if (command.FullName.Contains(TransportName))
					_commandTypes.Add(command.Name, command);
			}
		}

		public virtual ITransportCommand CreateCommand(string commandName)
		{
			if (!_commandTypes.TryGetValue(commandName, out var type))
				throw new NotImplementedException($"ITransportCommand: {commandName} was not implemented for {TransportName}.");

			return Activator.CreateInstance(type) as ITransportCommand;
		}
	}
}
