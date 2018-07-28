using Crispin.Infrastructure;

namespace Crispin.Tests
{
	public static class Utils
	{
		public static Act<T> AsAct<T>(this T self) where T : IEvent => new Act<T>
		{
			AggregateID = self.AggregateID,
			TimeStamp = self.TimeStamp,
			Data = self
		};
		
		public static Act<T> AsAct<T>(this T self, ToggleID id) where T : IEvent => new Act<T>
		{
			AggregateID = id,
			TimeStamp = self.TimeStamp,
			Data = self
		};
	}
}
