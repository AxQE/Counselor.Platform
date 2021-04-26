using System;
using System.Collections.Generic;
using System.Linq;

namespace Counselor.Platform.Utils
{
	static class TypeHelpers
	{
		public static IEnumerable<Type> GetTypeImplementations<TInterface>()
		{
			var types = from assemblies in AppDomain.CurrentDomain.GetAssemblies()
						from stepType in assemblies.GetTypes()
						where
							typeof(TInterface).IsAssignableFrom(stepType)
							&& !stepType.IsAbstract
							&& !stepType.IsInterface
						select stepType;

			return types;
		}
	}
}
