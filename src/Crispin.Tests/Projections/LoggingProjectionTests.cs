using System;
using System.Linq;
using Crispin.Infrastructure;
using Crispin.Projections;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Projections
{
	public class LoggingProjectionTests
	{
		private readonly LoggingProjection _projection;

		public LoggingProjectionTests()
		{
			_projection = new LoggingProjection();
		}

		[Fact]
		public void When_consuming_events()
		{
			var now = DateTime.Now;
			var first = new First { TimeStamp = now.AddSeconds(1) };
			var second = new Second { TimeStamp = now.AddSeconds(2) };

			_projection.Consume(first);
			_projection.Consume(second);

			_projection.Messages.Select(e => e.Message).ShouldBe(new[]
			{
				first.ToString(),
				second.ToString()
			});

			_projection.Messages.Select(e => e.TimeStamp).ShouldBe(new[]
			{
				now.AddSeconds(1),
				now.AddSeconds(2)
			});
		}

		public class First : Event {}
		public class Second : Event {}
	}
}
