using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Commands
{
	public interface ITransportCommandFactory
	{
		string TransportName { get; }
		ITransportCommand CreateCommand(string identificator);		
	}
}
