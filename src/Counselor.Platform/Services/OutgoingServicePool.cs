using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Counselor.Platform.Services
{
	public class OutgoingServicePool : IOutgoingServicePool
	{
		private readonly IServiceProvider _serviceProvider;

		private readonly ConcurrentDictionary<string, IOutgoingService> _outgoingServices = new ConcurrentDictionary<string, IOutgoingService>();

		public OutgoingServicePool(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;

			var services = from assembly in AppDomain.CurrentDomain.GetAssemblies()
						   from assemblyType in assembly.GetTypes()
						   where
							   typeof(IOutgoingService).IsAssignableFrom(assemblyType)
							   && !assemblyType.IsAbstract
							   && !assemblyType.IsInterface
						   select assemblyType;

			foreach (var service in services)
			{
				var concreteService = _serviceProvider.GetService(service) as IOutgoingService;
				_outgoingServices.TryAdd(concreteService.TransportSystemName, concreteService);
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
