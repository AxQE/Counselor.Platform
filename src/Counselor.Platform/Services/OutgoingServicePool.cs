using Counselor.Platform.Utils;
using System;
using System.Collections.Concurrent;

namespace Counselor.Platform.Services
{
	public class OutgoingServicePool : IOutgoingServicePool
	{
		private readonly IServiceProvider _serviceProvider;

		private readonly ConcurrentDictionary<string, IOutgoingService> _outgoingServices = new ConcurrentDictionary<string, IOutgoingService>();

		public OutgoingServicePool(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;

			foreach (var serviceType in TypeHelpers.GetTypeImplementations<IOutgoingService>())
			{
				var service = _serviceProvider.GetService(serviceType) as IOutgoingService;

				if (service != null)
					_outgoingServices.TryAdd(service.TransportSystemName, service);
			}
		}

		public IOutgoingService Resolve(string transport)
		{
			if (!_outgoingServices.TryGetValue(transport, out var service))
			{
				throw new ArgumentOutOfRangeException(nameof(transport), $"Service for {transport} not found in outgoing service pool.");
			}

			return service;
		}
	}
}
