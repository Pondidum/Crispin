using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CrispinClient.Statistics.Writers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Statistics.Writers
{
	public class BatchingWriterTests
	{
		private const int BatchSize = 8;

		private readonly ICrispinClient _client;
		private readonly BatchingWriter _writer;
		private readonly Fixture _fixture;

		public BatchingWriterTests()
		{
			_client = Substitute.For<ICrispinClient>();
			_writer = new BatchingWriter(_client, BatchSize);
			_fixture = new Fixture();
		}

		[Fact]
		public void When_a_statistic_is_written()
		{
			_writer.Write(_fixture.Create<Statistic>());

			_client.DidNotReceiveWithAnyArgs().SendStatistics(null);
		}

		[Fact]
		public void When_fewer_statistics_than_batch_size_are_written()
		{
			var stats = Enumerable
				.Range(0, BatchSize - 1)
				.Select(i => _fixture.Create<Statistic>())
				.ToList();

			stats.ForEach(_writer.Write);

			_client.DidNotReceiveWithAnyArgs().SendStatistics(null);
		}

		[Fact]
		public void When_more_statistics_than_batch_size_are_written()
		{
			var stats = Enumerable
				.Range(0, BatchSize)
				.Select(i => _fixture.Create<Statistic>())
				.ToList();

			stats.ForEach(_writer.Write);

			_client.Received().SendStatistics(Arg.Do<IEnumerable<Statistic>>(e => e.ShouldBe(stats)));
		}
	}
}
