using System;
using System.Collections.Generic;
using System.Linq;

namespace Counselor.Platform.Utils
{
	public static class TypeHelpers
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

		public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>() where TAttribute : Attribute
		{
			var attributedTypes = from assemblies in AppDomain.CurrentDomain.GetAssemblies()
								  from types in assemblies.GetTypes()
								  where
									  types.GetCustomAttributes(typeof(TAttribute), false).Any()
								  select types;

			return attributedTypes;
		}

		public static IEnumerable<TAttribute> GetAppliedAttributes<TAttribute>(Type type) where TAttribute : Attribute
		{
			var attributes = type.GetCustomAttributes(typeof(TAttribute), false);

			if (attributes.Any())
			{
				var attributesList = new List<TAttribute>(attributes.Length);

				foreach (var att in attributes)
				{
					attributesList.Add((TAttribute)att);
				}

				return attributesList;
			}

			throw new TypeAccessException($"Attribute of type {typeof(TAttribute)} not found on type {type}.");
		}
	}
}
