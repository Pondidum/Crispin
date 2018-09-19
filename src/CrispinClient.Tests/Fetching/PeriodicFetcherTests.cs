using System;
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
	public class PeriodicFetcherTests
	{
		private readonly Toggle[] _toggles;
		private readonly ICrispinClient _client;
		private readonly ITimeControl _timeControl;
		private readonly PeriodicFetcher _fetcher;

		public PeriodicFetcherTests()
		{
			_toggles = Enumerable.Range(0, 5).Select(i => new Toggle { ID = Guid.NewGuid(), Name = i.ToString() }).ToArray();

			_client = Substitute.For<ICrispinClient>();
			_client.GetAllToggles().Returns(_toggles);

			_timeControl = Substitute.For<ITimeControl>();

			_fetcher = new PeriodicFetcher(_client, TimeSpan.FromSeconds(5), _timeControl);
		}

		[Fact]
		public void The_initial_query_doesnt_block_construction()
		{
			_client.GetAllToggles().Returns(ci =>
			{
				Thread.Sleep(TimeSpan.FromSeconds(10));
				return _toggles;
			});

			Should.CompleteIn(() => new PeriodicFetcher(_client, TimeSpan.FromSeconds(1), _timeControl), TimeSpan.FromSeconds(1));
		}

		[Fact]
		public void The_first_fetch_blocks_until_a_query_has_been_made()
		{
			_client.GetAllToggles().Returns(ci =>
			{
				Thread.Sleep(TimeSpan.FromSeconds(1));
				return _toggles;
			});

			_fetcher.GetAllToggles().ShouldBe(_toggles.ToDictionary(t => t.ID));
		}
	}
}
