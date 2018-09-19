using System;
using System.Linq;
using CrispinClient.Fetching;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Fetching
{
	public class OneTimeFetcherTests
	{
		private readonly ICrispinClient _client;
		private readonly Toggle[] _toggles;
		private readonly OneTimeFetcher _fetcher;

		public OneTimeFetcherTests()
		{
			_toggles = Enumerable.Range(0, 5).Select(i => new Toggle { ID = Guid.NewGuid(), Name = i.ToString() }).ToArray();

			_client = Substitute.For<ICrispinClient>();
			_client.GetAllToggles().Returns(_toggles);

			_fetcher = new OneTimeFetcher(_client);
		}

		[Fact]
		public void Fetching_doesnt_happen_on_construction()
		{
			_client.DidNotReceive().GetAllToggles();
		}

		[Fact]
		public void When_fetching_toggles()
		{
			_fetcher.GetAllToggles().ShouldBe(_toggles.ToDictionary(t => t.ID));
		}

		[Fact]
		public void When_fetching_toggles_multiple_times()
		{
			_fetcher.GetAllToggles();
			_fetcher.GetAllToggles();

			_client.Received(1).GetAllToggles();
		}
	}
}
