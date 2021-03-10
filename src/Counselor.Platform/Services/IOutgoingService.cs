﻿using Counselor.Platform.Entities;
using System;
using System.Threading.Tasks;

namespace Counselor.Platform.Services
{
	public interface IOutgoingService
	{
		public string TransportSystemName { get; }
		Task SendAsync(Message message, int userId);
	}
}
