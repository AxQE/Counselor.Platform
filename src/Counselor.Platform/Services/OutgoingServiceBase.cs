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

		public OutgoingServiceBase(
			ILogger<OutgoingServiceBase> logger,
			IOptions<TransportOptions> options,
			ConnectionsRepository connections)
		{
			_logger = logger;
			_options = options.Value;
			_connections = connections;
		}

		protected abstract Task SendMessageToTransportAsync(string connectionId, string payload);

		public async Task SendAsync(string payload, int userId)
		{
			try
			{
				_logger.LogDebug($"Send message to {TransportSystemName}. Message: {payload}. User: {userId}.");

				var connectionId = await _connections.GetConnectionIdAsync(userId, TransportSystemName);
				await SendMessageToTransportAsync(connectionId, payload);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Send message to {TransportSystemName} failed. Message: {payload}. User: {userId}.");
			}
		}
	}
}
