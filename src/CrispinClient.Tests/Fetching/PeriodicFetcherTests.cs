using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrispinClient.Fetching;
using CrispinClient.Infrastructure;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Fetching
{
	public class PeriodicFetcherTests : IDisposable
	{
		private readonly Toggle[] _toggles;
		private readonly ICrispinClient _client;
		private readonly ITimeControl _timeControl;
		private PeriodicFetcher _fetcher;

		public PeriodicFetcherTests()
		{
			_toggles = Enumerable.Range(0, 5).Select(i => new Toggle { ID = Guid.NewGuid(), Name = i.ToString() }).ToArray();

			_client = Substitute.For<ICrispinClient>();

			_timeControl = Substitute.For<ITimeControl>();
			_timeControl
				.Delay(Arg.Any<TimeSpan>(), Arg.Any<CancellationToken>())
				.Returns(Task.CompletedTask);
		}

		private PeriodicFetcher CreateFetcher() => _fetcher = new PeriodicFetcher(_client, TimeSpan.FromSeconds(5), _timeControl);

		[Fact]
		public void The_initial_query_doesnt_block_construction()
		{
			_client.GetAllToggles().Returns(ci =>
			{
				Thread.Sleep(TimeSpan.FromSeconds(2));
				return _toggles;
			});

			Should.CompleteIn(() => CreateFetcher(), TimeSpan.FromSeconds(1));
		}

		[Fact]
		public async Task The_first_fetch_blocks_until_a_query_has_been_made()
		{
			_client.GetAllToggles().Returns(_toggles);

			var fetcher = CreateFetcher();
			var toggles = await fetcher.GetAllToggles();

			toggles.ShouldBe(_toggles.ToDictionary(t => t.ID));
		}

		[Fact]
		public async Task When_the_background_fetch_fails()
		{
			_client.GetAllToggles().Returns(
				ci => _toggles,
				ci => throw new TimeoutException()
			);

			var fetcher = CreateFetcher();
			var toggles = await fetcher.GetAllToggles();

			toggles.ShouldBe(_toggles.ToDictionary(t => t.ID));
		}


		[Fact]
		public async Task When_the_background_fetch_fails_and_a_subsequent_call_succeeds()
		{
			var finished = new ManualResetEventSlim();
			var currentStep = 0;

			_client
				.GetAllToggles()
				.Returns(ci =>
				{
					currentStep++;

					if (currentStep == 1)
						return _toggles.Take(2).ToArray();

					if (currentStep == 2)
						throw new TimeoutException();

					if (currentStep > 3)
						finished.Set();

					return _toggles;
				});

			var fetcher = CreateFetcher();
			finished.Wait(TimeSpan.FromSeconds(2));

			var toggles = await fetcher.GetAllToggles();

			toggles.ShouldBe(_toggles.ToDictionary(t => t.ID));
		}

		[Fact]
		public async Task When_the_initial_query_fails()
		{
			var finished = new ManualResetEventSlim();
			var currentStep = 0;

			_client
				.GetAllToggles()
				.Returns(ci =>
				{
					currentStep++;

					if (currentStep == 1)
						throw new TimeoutException();

					if (currentStep > 2)
						finished.Set();

					return _toggles;
				});

			var fetcher = CreateFetcher();
			finished.Wait(TimeSpan.FromSeconds(2));
			var toggles = await fetcher.GetAllToggles();

			toggles.ShouldBe(_toggles.ToDictionary(t => t.ID));
		}

		public void Dispose()
		{
			_fetcher?.Dispose();
		}
	}
}
