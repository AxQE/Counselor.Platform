using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Counselor.Platform.Services
{
	public class OutgoingServicePool : IOutgoingServicePool
	{
		private readonly ConcurrentDictionary<string, IOutgoingService> _outgoingServices = new ConcurrentDictionary<string, IOutgoingService>();

		public OutgoingServicePool(IEnumerable<IOutgoingService> outgoingServices)
		{
			foreach (var service in outgoingServices)
			{
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
