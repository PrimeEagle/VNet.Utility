using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;


namespace VNet.Utility
{
	public static class ReflectionUtility
	{
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

			if (!IsValidCSharpIdentifier(className))
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

		public static TypeBuilder AddInterface(this TypeBuilder tb, Type interfaceType)
		{
			if (interfaceType is null) throw new ArgumentNullException();

			tb.AddInterfaceImplementation(interfaceType);

			return tb;
		}

		public static TypeBuilder AddMethod(this TypeBuilder tb, string name,
											MethodAttributes methodAttributes = MethodAttributes.Public,
											Type returnType = null,
											Type[] parameterTypes = null,
											ILGenerator ilGenerator = null,
											Type interfaceTypeForSourceMethod = null,
											IEnumerable<CustomAttributeBuilder> attributes = null)
		{

			returnType ??= typeof(void);
			parameterTypes ??= Array.Empty<Type>();

			var methodName = name;

			if (string.IsNullOrEmpty(methodName))
			{
				methodName = "Method" + StringUtility.CreateRandomDigits(4);
			}

			if (!IsValidCSharpIdentifier(methodName))
			{
				throw new ArgumentException();
			}

			var ti = CultureInfo.CurrentCulture.TextInfo;

			var titleCaseName = ti.ToTitleCase(methodName);

			var mb =
				tb.DefineMethod(titleCaseName,
					methodAttributes,
					returnType,
					parameterTypes);

			if (ilGenerator is null)
			{
				ilGenerator = mb.GetILGenerator();
				ilGenerator.EmitWriteLine($"Method {titleCaseName} called.");
				ilGenerator.Emit(OpCodes.Ret);
			}

			if (interfaceTypeForSourceMethod is not null)
			{
				var executeMethod = interfaceTypeForSourceMethod.GetMethod(titleCaseName);
				if (executeMethod is not null) tb.DefineMethodOverride(mb, executeMethod);
			}

			if (attributes is null) return tb;

			foreach (var a in attributes)
			{
				mb.SetCustomAttribute(a);
			}

			return tb;
		}

		public static TypeBuilder AddProperty(this TypeBuilder tb, string name, Type type,
												FieldAttributes propertyAttributes = FieldAttributes.Public,
												IEnumerable<CustomAttributeBuilder> attributes = null)
		{
			if (type is null) throw new ArgumentNullException();

			var propertyName = name;

			if (string.IsNullOrEmpty(propertyName))
			{
				propertyName = "Property" + StringUtility.CreateRandomDigits(4);
			}

			if (!IsValidCSharpIdentifier(propertyName))
			{
				throw new ArgumentException();
			}

			var ti = CultureInfo.CurrentCulture.TextInfo;

			var titleCaseName = ti.ToTitleCase(propertyName);
			var camelCaseName = char.ToLowerInvariant(titleCaseName[0]) + titleCaseName[1..];

			var customerNameBuilder = tb.DefineField(camelCaseName,
												type,
												FieldAttributes.Private);

			var propertyBuilder = tb.DefineProperty(titleCaseName,
															 PropertyAttributes.HasDefault,
															 type,
															 null);

			var getSetAttr =
				MethodAttributes.Public | MethodAttributes.SpecialName |
					MethodAttributes.HideBySig;

			var getMethodBuilder =
				tb.DefineMethod($"get_{titleCaseName}",
										   getSetAttr,
										   type,
										   Type.EmptyTypes);

			var getILGenerator = getMethodBuilder.GetILGenerator();

			getILGenerator.Emit(OpCodes.Ldarg_0);
			getILGenerator.Emit(OpCodes.Ldfld, customerNameBuilder);
			getILGenerator.Emit(OpCodes.Ret);

			var setMethodBuilder =
				tb.DefineMethod($"set_{titleCaseName}",
										   getSetAttr,
										   null,
										   new Type[] { type });

			var setILGenerator = setMethodBuilder.GetILGenerator();

			setILGenerator.Emit(OpCodes.Ldarg_0);
			setILGenerator.Emit(OpCodes.Ldarg_1);
			setILGenerator.Emit(OpCodes.Stfld, customerNameBuilder);
			setILGenerator.Emit(OpCodes.Ret);

			propertyBuilder.SetGetMethod(getMethodBuilder);
			propertyBuilder.SetSetMethod(setMethodBuilder);

			if (attributes is null) return tb;

			foreach (var a in attributes)
			{
				propertyBuilder.SetCustomAttribute(a);
			}

			return tb;
		}

		public static bool IsValidCSharpIdentifier(string identifier)
		{
			return SyntaxFacts.IsValidIdentifier(identifier);
		}
	}
}