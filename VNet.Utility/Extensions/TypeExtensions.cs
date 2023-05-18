using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace VNet.Utility.Extensions
{
	public static class TypeExtensions
	{
		public static IEnumerable<Type> ClassesWithAttribute<T>(this Type type) where T : Attribute
		{
			return type.GetCustomAttributes(typeof(T), false).Select(a => a.GetType());
		}

		public static IEnumerable<MethodInfo> MethodsWithAttribute<T>(this Type type) where T : Attribute
		{
			return type.GetMethods().Where(n => n.GetCustomAttributes(typeof(T), false).Length > 0);
		}

		public static IEnumerable<PropertyInfo> PropertiesWithAttribute<T>(this Type type) where T : Attribute
		{
			return type.GetProperties().Where(n => n.GetCustomAttributes(typeof(T), false).Length > 0);
		}
	}
}