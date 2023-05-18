using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace VNet.Utility.Extensions
{
	public static class ICustomAttributeProviderExtensions
	{
		public static IEnumerable<T> Attributes<T>(this ICustomAttributeProvider type)
			where T : Attribute
		{
			var result = new List<T>();
			var c = type.GetCustomAttributes(typeof(T), false).Select(a => (T)a);

			result.AddRange(c);

			return result;
		}
	}
}