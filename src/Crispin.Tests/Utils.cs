using System;
using Crispin.Infrastructure;

namespace Crispin.Tests
{
	public static class Utils
	{
		public static Act<T> AsAct<T>(this T self) => new Act<T>
		{
			TimeStamp = DateTime.Now,
			Data = self
		};
		
		public static Act<T> AsAct<T>(this T self, ToggleID id) => new Act<T>
		{
			AggregateID = id,
			TimeStamp = DateTime.Now,
			Data = self
		};
	}
}
