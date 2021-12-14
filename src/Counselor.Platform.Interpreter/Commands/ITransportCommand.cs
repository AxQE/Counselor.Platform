using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Commands
{
	public interface ITransportCommand
	{
		object Parameter { get; set; }
		string ConnectionId { get; set; }
		Task ExecuteAsync(object transportClient);
	}
}
