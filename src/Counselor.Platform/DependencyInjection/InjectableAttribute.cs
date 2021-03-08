using Microsoft.Extensions.DependencyInjection;
using System;

namespace Counselor.Platform.DependencyInjection
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class InjectableAttribute : Attribute
	{
		public ServiceLifetime Lifetime { get; set; }

		public InjectableAttribute(ServiceLifetime lifetime = ServiceLifetime.Transient)
		{
			Lifetime = lifetime;
		}
	}
}
