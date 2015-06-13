﻿//  This file is part of YamlDotNet - A .NET library for YAML.
//  Copyright (c) Antoine Aubry and contributors

//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace YamlDotNet
{
	/// <summary>
	/// Mock SerializableAttribute to avoid having to add #if all over the place
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class SerializableAttribute : Attribute { }

	internal static class ReflectionExtensions
	{
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		/// <summary>
		/// Determines whether the specified type has a default constructor.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<c>true</c> if the type has a default constructor; otherwise, <c>false</c>.
		/// </returns>
		public static bool HasDefaultConstructor(this Type type)
		{
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var constructor = type.GetConstructor(bindingFlags, null, Type.EmptyTypes, null);
			return type.IsValueType || constructor != null;
		}

		public static bool IsAssignableFrom(this Type type, Type source)
		{
			return type.IsAssignableFrom(source);
		}

		public static TypeCode GetTypeCode(this Type type)
		{
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}

			if (type == typeof(bool))
			{
				return TypeCode.Boolean;
			}
			else if (type == typeof(char))
			{
				return TypeCode.Char;
			}
			else if (type == typeof(sbyte))
			{
				return TypeCode.SByte;
			}
			else if (type == typeof(byte))
			{
				return TypeCode.Byte;
			}
			else if (type == typeof(short))
			{
				return TypeCode.Int16;
			}
			else if (type == typeof(ushort))
			{
				return TypeCode.UInt16;
			}
			else if (type == typeof(int))
			{
				return TypeCode.Int32;
			}
			else if (type == typeof(uint))
			{
				return TypeCode.UInt32;
			}
			else if (type == typeof(long))
			{
				return TypeCode.Int64;
			}
			else if (type == typeof(ulong))
			{
				return TypeCode.UInt64;
			}
			else if (type == typeof(float))
			{
				return TypeCode.Single;
			}
			else if (type == typeof(double))
			{
				return TypeCode.Double;
			}
			else if (type == typeof(decimal))
			{
				return TypeCode.Decimal;
			}
			else if (type == typeof(DateTime))
			{
				return TypeCode.DateTime;
			}
			else if (type == typeof(String))
			{
				return TypeCode.String;
			}
			else
			{
				return TypeCode.Object;
			}
		}

		public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
		{
            if (type == null) throw new ArgumentNullException("type");
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}

		public static IEnumerable<MethodInfo> GetPublicMethods(this Type type)
		{
            if (type == null) throw new ArgumentNullException("type");
			return type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
		}

		public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] parameterTypes)
		{
            if (type == null) throw new ArgumentNullException("type");
			return type.GetMethod(name, BindingFlags.Public | BindingFlags.Static, null, parameterTypes, null);
		}

		public static Exception Unwrap(this TargetInvocationException ex)
		{
			return ex.InnerException;
		}
	}

	internal enum TypeCode
	{
		Empty = 0,
		Object = 1,
		DBNull = 2,
		Boolean = 3,
		Char = 4,
		SByte = 5,
		Byte = 6,
		Int16 = 7,
		UInt16 = 8,
		Int32 = 9,
		UInt32 = 10,
		Int64 = 11,
		UInt64 = 12,
		Single = 13,
		Double = 14,
		Decimal = 15,
		DateTime = 16,
		String = 18,
	}

	internal static class StandardRegexOptions
	{
		public const RegexOptions Compiled = RegexOptions.None;
	}

	internal abstract class DBNull
	{
		private DBNull() {}
	}

	internal sealed class CultureInfoAdapter : CultureInfo
	{
		private readonly IFormatProvider _provider;

		public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider)
			: base(baseCulture.Name)
		{
			_provider = provider;
		}

		public override object GetFormat(Type formatType)
		{
			return _provider.GetFormat(formatType);
		}
	}
}
