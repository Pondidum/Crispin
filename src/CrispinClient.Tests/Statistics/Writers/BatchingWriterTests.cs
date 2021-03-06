using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using CrispinClient.Infrastructure;
using CrispinClient.Statistics.Writers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Statistics.Writers
{
	public class BatchingWriterTests : IDisposable
	{
		private const int BatchSize = 8;

		private readonly ICrispinClient _client;

		private readonly BatchingWriter _writer;
		private readonly Fixture _fixture;
		private Func<Task> _timer;


		public BatchingWriterTests()
		{
			var timeControl = Substitute.For<ITimeControl>();
			timeControl
				.When(t => t.Every(Arg.Any<TimeSpan>(), Arg.Any<Func<Task>>()))
				.Do(ci => _timer = ci.Arg<Func<Task>>());

			_client = Substitute.For<ICrispinClient>();
			_writer = new BatchingWriter(_client, time: timeControl, batchSize: BatchSize);
			_fixture = new Fixture();
		}

		public void Dispose()
		{
			_writer.Dispose();
		}

		[Fact]
		public async Task When_a_statistic_is_written()
		{
			await _writer.Write(_fixture.Create<Statistic>());

			await _client.DidNotReceiveWithAnyArgs().SendStatistics(null);
		}

		[Fact]
		public async Task When_fewer_statistics_than_batch_size_are_written()
		{
			var stats = Enumerable
				.Range(0, BatchSize - 1)
				.Select(i => _fixture.Create<Statistic>())
				.ToList();

			foreach (var statistic in stats)
				await _writer.Write(statistic);

			await _client.DidNotReceiveWithAnyArgs().SendStatistics(null);
		}

		[Fact]
		public async Task When_more_statistics_than_batch_size_are_written()
		{
			var stats = Enumerable
				.Range(0, BatchSize)
				.Select(i => _fixture.Create<Statistic>())
				.ToList();

			foreach (var statistic in stats)
				await _writer.Write(statistic);

			await _client.Received().SendStatistics(Arg.Do<IEnumerable<Statistic>>(e => e.ShouldBe(stats)));
		}

		[Fact]
		public async Task When_a_small_batch_expires()
		{
			var stats = Enumerable
				.Range(0, 2)
				.Select(i => _fixture.Create<Statistic>())
				.ToList();

			foreach (var statistic in stats)
				await _writer.Write(statistic);

			await _timer();

			await _client.Received().SendStatistics(Arg.Do<IEnumerable<Statistic>>(e => e.ShouldBe(stats)));
		}

		[Fact]
		public async Task Statistics_dont_get_lost()
		{
			var seen = 0;
			_client
				.When(c => c.SendStatistics(Arg.Any<IEnumerable<Statistic>>()))
				.Do(ci => seen += ci.Arg<IEnumerable<Statistic>>().Count());

			var stats = new List<Statistic>();
			var run = true;

			var consume = Task.Run(async () =>
			{
				while (run)
				{
					await _timer();
					await Task.Delay(10);
				}
			});

			var produce = Task.Run(async () =>
			{
				while (run)
				{
					var stat = _fixture.Create<Statistic>();
					stats.Add(stat);
					await _writer.Write(stat);

					await Task.Delay(1);
				}
			});

			await Task.Delay(5000);
			run = false;

			await Task.WhenAll(produce, consume);
			await _timer();

			seen.ShouldBe(stats.Count);
		}
	}
}
