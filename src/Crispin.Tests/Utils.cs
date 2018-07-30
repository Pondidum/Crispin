using System;
using Crispin.Infrastructure;

namespace Crispin.Tests
{
	public static class Utils
	{
		public static Event<T> AsAct<T>(this T self) => new Event<T>
		{
			TimeStamp = DateTime.Now,
			Data = self
		};
		
		public static Event<T> AsAct<T>(this T self, ToggleID id) => new Event<T>
		{
			AggregateID = id,
			TimeStamp = DateTime.Now,
			Data = self
		};
	}
}
