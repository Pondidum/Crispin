using System;
using System.Threading.Tasks;
using CrispinClient.Infrastructure;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Infrastructure
{
	public class RealTimeControlTests
	{
		private readonly RealTimeControl _timeControl;

		public RealTimeControlTests()
		{
			_timeControl = new RealTimeControl();
		}

		[Fact]
		public async Task Repeating_works()
		{
			var count = 0;
			var stop = _timeControl.Every(TimeSpan.FromMilliseconds(10), () => count++);

			await Task.Delay(TimeSpan.FromMilliseconds(50));
			await stop();

			count.ShouldBeGreaterThan(0);
		}

		[Fact]
		public async Task Stopping_a_repeat_works()
		{
			var count = 0;
			var stop = _timeControl.Every(TimeSpan.FromMilliseconds(5), () => count++);

			await stop();

			var current = count;
			await Task.Delay(TimeSpan.FromMilliseconds(50));

			count.ShouldBe(current);
		}
	}
}
