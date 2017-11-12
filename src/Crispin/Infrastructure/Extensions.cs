using System;
using System.Collections.Generic;

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
	}
}
