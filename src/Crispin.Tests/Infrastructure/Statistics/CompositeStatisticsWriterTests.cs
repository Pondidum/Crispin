using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;
using NSubstitute;
using Xunit;

namespace Crispin.Tests.Infrastructure.Statistics
{
	public class CompositeStatisticsWriterTests
	{
		private readonly LoggingStatisticsWriter _first;
		private readonly StatsdStatisticsWriter _second;
		private readonly CompositeStatisticsWriter _writer;

		public CompositeStatisticsWriterTests()
		{
			_first = Substitute.For<LoggingStatisticsWriter>();
			_second = Substitute.For<StatsdStatisticsWriter>();

			_writer = new CompositeStatisticsWriter(_first, _second);
		}

		[Fact]
		public async Task When_write_is_called()
		{
			const string key = "some key";
			const string value = "some value";

			await _writer.Write(key, value);

			await _first.Received().Write(key, value);
			await _second.Received().Write(key, value);
		}

		[Fact]
		public async Task When_writeCount_is_called()
		{
			const string key = "some key";

			await _writer.WriteCount(key);

			await _first.Received().WriteCount(key);
			await _second.Received().WriteCount(key);
		}
	}
}
