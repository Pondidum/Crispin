using System;
using System.Collections.Generic;
using System.Reflection;

namespace Crispin.Infrastructure
{
	public static class Extensions
	{
		public static void Each<T>(this IEnumerable<T> self, Action<T> action)
		{
			foreach (var item in self)
				action(item);
		}

		public static bool EqualsIgnore(this string first, string second) =>
			string.Equals(first, second, StringComparison.OrdinalIgnoreCase);

		public static void Match<T>(this T? self, Action<T> hasValue, Action noValue) where T : struct
		{
			if (self.HasValue)
				hasValue(self.Value);
			else
				noValue();
		}

		public static void Match<T>(this T self, Action<T> hasValue, Action noValue) where T : class
		{
			if (self != null)
				hasValue(self);
			else
				noValue();
		}

		public static T As<T>(this object target)
		{
			return (T)target;
		}

		public static bool Closes(this Type type, Type openType)
		{
			if (type == null) return false;

			var typeInfo = type.GetTypeInfo();

			if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == openType) return true;

			foreach (var @interface in type.GetInterfaces())
			{
				if (@interface.Closes(openType)) return true;
			}

			var baseType = typeInfo.BaseType;
			if (baseType == null) return false;

			var baseTypeInfo = baseType.GetTypeInfo();

			var closes = baseTypeInfo.IsGenericType && baseType.GetGenericTypeDefinition() == openType;
			if (closes) return true;

			return typeInfo.BaseType?.Closes(openType) ?? false;
		}

		public static T CloseAndBuildAs<T>(this Type openType, object ctorArgument, params Type[] parameterTypes)
		{
			var closedType = openType.MakeGenericType(parameterTypes);
			return (T)Activator.CreateInstance(closedType, ctorArgument);
		}
	}
}
