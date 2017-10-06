﻿using System;
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
	}
}
