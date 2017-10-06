using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;
using NSubstitute;
using Xunit;

namespace Crispin.Tests.Infrastructure.Statistics
{
	public class StatisticsBehaviorTests
	{
		private readonly IStatisticsWriter _writer;
		private readonly StatisticsBehavior<Request, Response> _decorator;
		private readonly List<IStatisticGenerator<Request, Response>> _generators;

		public StatisticsBehaviorTests()
		{
			_generators = new List<IStatisticGenerator<Request, Response>>();
			_writer = Substitute.For<IStatisticsWriter>();
			_decorator = new StatisticsBehavior<Request, Response>(_writer, _generators);
		}

		[Fact]
		public async Task When_there_isnt_a_statistics_generator_for_the_message()
		{
			_generators.Clear();

			await _decorator.Handle(new Request(), () => Task.FromResult(new Response()));

			await _writer.DidNotReceiveWithAnyArgs().WriteCount(Arg.Any<IStat>());
		}

		[Fact]
		public async Task When_there_is_a_generator()
		{
			_generators.Add(CreateGenerator(Substitute.For<IStat>()));

			await _decorator.Handle(new Request(), () => Task.FromResult(new Response()));

			await _writer.Received().WriteCount(Arg.Any<IStat>());
		}

		[Fact]
		public async Task When_there_is_are_multiple_generators()
		{
			var stat1 = Substitute.For<IStat>();
			var stat2 = Substitute.For<IStat>();

			_generators.Add(CreateGenerator(stat1));
			_generators.Add(CreateGenerator(stat2));

			await _decorator.Handle(new Request(), () => Task.FromResult(new Response()));

			await _writer.Received().WriteCount(stat1);
			await _writer.Received().WriteCount(stat2);
		}

		private static IStatisticGenerator<Request, Response> CreateGenerator(IStat stat)
		{
			var generator = Substitute.For<IStatisticGenerator<Request, Response>>();
			generator
				.Write(
					Arg.Do<IStatisticsWriter>(w => w.WriteCount(stat)),
					Arg.Any<Request>(),
					Arg.Any<Response>())
				.Returns(Task.CompletedTask);

			return generator;
		}

		public class Request
		{
		}

		public class Response
		{
		}
	}
}
