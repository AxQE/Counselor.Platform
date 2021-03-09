using Counselor.Platform.Core;
using Counselor.Platform.Options;
using Counselor.Platform.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Services
{
	public abstract class OutgoingServiceBase : IOutgoingService
	{
		public string TransportSystemName => _options.TransportSystemName;
		private readonly ILogger<OutgoingServiceBase> _logger;
		private readonly TransportOptions _options;
		private readonly ConnectionsRepository _connections;
		protected abstract Func<string, string, Task> SendMessageToTransportAsync { get; }

		public OutgoingServiceBase(
			ILogger<OutgoingServiceBase> logger, 
			IOptions<TransportOptions> options,
			ConnectionsRepository connections)
		{
			_logger = logger;
			_options = options.Value;
			_connections = connections;
		}

		public async Task SendAsync(IMessage message, int userId)
		{
			try
			{
				_logger.LogDebug($"Send message to {TransportSystemName}. Message: {message.Payload}. User: {userId}.");

				var connectionId = await _connections.GetConnectionIdAsync(userId, TransportSystemName);
				await SendMessageToTransportAsync(connectionId, message.Payload);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Send message to {TransportSystemName} failed. Message: {message.Payload}. User: {userId}.");
			}
		}
	}
}
