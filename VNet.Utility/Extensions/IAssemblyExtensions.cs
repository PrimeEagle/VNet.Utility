using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using VNet.Testing;

// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace VNet.Utility.Extensions
{
	public static class IAssemblyExtensions
	{
		public static IEnumerable<Type> ClassesWithAttribute<T>(this IAssembly assembly) where T : Attribute
		{
			return assembly.GetTypes()
				.Where(m => m.GetCustomAttributes(typeof(T), false).Length > 0);
		}

		public static IEnumerable<MethodInfo> MethodsWithAttribute<T>(this IAssembly assembly) where T : Attribute
		{
			return assembly.GetTypes()
				.SelectMany(t => t.GetMethods())
				.Where(n => n.GetCustomAttributes(typeof(T), false).Length > 0);
		}

		public static IEnumerable<Attribute> Attributes<T>(this IAssembly assembly, ReflectionSearchScope search)
			where T : Attribute
		{
			var result = new List<Attribute>();

			if (search is ReflectionSearchScope.All or ReflectionSearchScope.Classes)
			{
				var c = assembly.GetTypes()
					.SelectMany(c => c.GetCustomAttributes(typeof(T), false).Select(a => (T)a));

				result.AddRange(c);
			}

			if (search is ReflectionSearchScope.All or ReflectionSearchScope.Methods)
			{
				var m = assembly.GetTypes()
					.SelectMany(t => t.GetMethods())
					.SelectMany(c => c.GetCustomAttributes(typeof(T), false).Select(a => (T)a));

				result.AddRange(m);
			}

			if (search is ReflectionSearchScope.All or ReflectionSearchScope.Properties)
			{
				var p = assembly.GetTypes()
					.SelectMany(t => t.GetProperties())
					.SelectMany(c => c.GetCustomAttributes(typeof(T), false).Select(a => (T)a));

				result.AddRange(p);
			}

			return result;
		}

		public static IEnumerable<PropertyInfo> PropertiesWithAttribute<T>(this IAssembly assembly) where T : Attribute
		{
			return assembly.GetTypes()
				.SelectMany(t => t.GetProperties())
				.Where(n => n.GetCustomAttributes(typeof(T), false).Length > 0);
		}

		public static TypeBuilder CreateClass(ModuleBuilder moduleBuilder, string name, TypeAttributes typeAttributes = TypeAttributes.NotPublic
																							| TypeAttributes.Class
																							| TypeAttributes.AutoClass
																							| TypeAttributes.AnsiClass
																							| TypeAttributes.BeforeFieldInit
																							| TypeAttributes.AutoLayout,
															IEnumerable<CustomAttributeBuilder> attributes = null)
		{
			var className = name;

			if (string.IsNullOrEmpty(className))
			{
				className = "Class" + StringUtility.CreateRandomDigits(4);
			}

			if (!ReflectionUtility.IsValidCSharpIdentifier(className))
			{
				throw new ArgumentException();
			}

			var ti = CultureInfo.CurrentCulture.TextInfo;

			var titleCaseName = ti.ToTitleCase(className);

			var tb = moduleBuilder.DefineType(titleCaseName,
				typeAttributes
				, null);

			if (attributes is null) return tb;

			foreach (var a in attributes)
			{
				tb.SetCustomAttribute(a);
			}

			return tb;
		}
	}
}