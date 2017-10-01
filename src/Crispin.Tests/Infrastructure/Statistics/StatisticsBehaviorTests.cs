using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;
using NSubstitute;
using Xunit;

namespace Crispin.Tests.Infrastructure.Statistics
{
	public class StatisticsBehaviorTests
	{
		private readonly IStatisticsWriter _writer;

		public StatisticsBehaviorTests()
		{
			_writer = Substitute.For<IStatisticsWriter>();
		}

		[Fact]
		public async Task When_the_request_and_response_dont_implement_IStatisticGenerator()
		{
			var decorator = new StatisticsBehavior<RequestWithout, ResponseWithout>(_writer);
			await decorator.Handle(new RequestWithout(), () => Task.FromResult(new ResponseWithout()));

			await _writer.DidNotReceiveWithAnyArgs().Write(null, null);
		}

		[Fact]
		public async Task When_the_request_implements_IStatisticGenerator()
		{
			var decorator = new StatisticsBehavior<RequestWith, ResponseWithout>(_writer);
			await decorator.Handle(new RequestWith(), () => Task.FromResult(new ResponseWithout()));

			await _writer.Received().Write("Request", "0");
		}

		[Fact]
		public async Task When_the_response_implements_IStatisticGenerator()
		{
			var decorator = new StatisticsBehavior<RequestWithout, ResponseWith>(_writer);
			await decorator.Handle(new RequestWithout(), () => Task.FromResult(new ResponseWith()));

			await _writer.Received().Write("Response", "0");
		}

		[Fact]
		public async Task When_both_request_and_response_implement_IStatisticGenerator()
		{
			var decorator = new StatisticsBehavior<RequestWith, ResponseWith>(_writer);
			await decorator.Handle(new RequestWith(), () => Task.FromResult(new ResponseWith()));

			await _writer.Received().Write("Request", "0");
			await _writer.Received().Write("Response", "0");
		}

		private class RequestWithout
		{
		}

		private class RequestWith : IStatisticGenerator
		{
			private int _count = 0;

			public async Task Write(IStatisticsWriter writer)
			{
				await writer.Write("Request", _count.ToString());
				_count++;
			}
		}

		private class ResponseWithout
		{
		}

		private class ResponseWith : IStatisticGenerator
		{
			private int _count = 0;

			public async Task Write(IStatisticsWriter writer)
			{
				await writer.Write("Response", _count.ToString());
				_count++;;
			}
		}
	}
}
