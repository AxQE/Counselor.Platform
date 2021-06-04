using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Commands
{
	public interface ITransportCommand
	{
		Task ExecuteAsync(object transportClient, string connectionId, object commandParameter);
	}
}
