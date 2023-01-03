using Counselor.Platform.Interpreter.Commands;

namespace Counselor.Platform.Worker.Transport.Web.Commands
{
	public class WebCommandFactory : TransportCommandFactory
	{
		public WebCommandFactory(string transportName) : base(transportName)
		{
		}
	}
}
