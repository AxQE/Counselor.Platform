using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Core
{
	class OutgoingServicePool : IOutgoingServicesPool
	{
		private readonly Dictionary<string, IOutgoingService> _outgoingServices;

		public OutgoingServicePool()
		{
			_outgoingServices = new Dictionary<string, IOutgoingService>();
		}

		public void Register(IOutgoingService outgoingService)
		{
			_outgoingServices.Add(outgoingService.SystemName, outgoingService);
		}

		public IOutgoingService Resolve(IMessage message)
		{			
			throw new NotImplementedException();
		}
	}
}
