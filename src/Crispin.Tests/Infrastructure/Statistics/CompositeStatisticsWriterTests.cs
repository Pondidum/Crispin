using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;
using NSubstitute;
using Xunit;

namespace Crispin.Tests.Infrastructure.Statistics
{
	public class CompositeStatisticsWriterTests
	{
		private readonly IStatisticsWriter _first;
		private readonly IStatisticsWriter _second;
		private readonly CompositeStatisticsWriter _writer;

		public CompositeStatisticsWriterTests()
		{
			_first = Substitute.For<IStatisticsWriter>();
			_second = Substitute.For<IStatisticsWriter>();

			_writer = new CompositeStatisticsWriter(new IStatisticsWriter[]
			{
				_first,
				_second
			});
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
