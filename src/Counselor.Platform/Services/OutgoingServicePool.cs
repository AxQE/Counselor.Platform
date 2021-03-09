using Counselor.Platform.Entities;
using System;
using System.Collections.Generic;

namespace Counselor.Platform.Services
{
	class OutgoingServicePool : IOutgoingServicePool
	{
		private readonly Dictionary<string, IOutgoingService> _outgoingServices;

		public OutgoingServicePool()
		{
			_outgoingServices = new Dictionary<string, IOutgoingService>();
		}

		public void Register(IOutgoingService outgoingService)
		{
			_outgoingServices.Add(outgoingService.TransportSystemName, outgoingService);
		}

		public IOutgoingService Resolve(Message message)
		{			
			throw new NotImplementedException();
		}
	}
}
