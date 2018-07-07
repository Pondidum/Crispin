using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Handlers.GetAll;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class GetAllTogglesHandlerTests
	{
		private readonly IStorageSession _session;
		private readonly GetAllTogglesHandler _handler;

		public GetAllTogglesHandlerTests()
		{
			_session = Substitute.For<IStorageSession>();
			_handler = new GetAllTogglesHandler(_session);
		}

		[Fact]
		public void When_there_is_no_projection_registered()
		{
			_session
				.LoadProjection<AllTogglesProjection>()
				.Throws(new ProjectionNotRegisteredException("wat"));

			Should.Throw<ProjectionNotRegisteredException>(
				() => _handler.Handle(new GetAllTogglesRequest())
			);
		}
		
		[Fact]
		public async Task When_there_are_no_toggles_in_the_projection()
		{
			var projection = new AllTogglesProjection();
			_session.LoadProjection<AllTogglesProjection>().Returns(projection);

			var response = await _handler.Handle(new GetAllTogglesRequest());

			response.Toggles.ShouldBeEmpty();
		}

		[Fact]
		public async Task When_there_are_toggles_in_the_projection()
		{
			var projection = new AllTogglesProjection();
			projection.Consume(new ToggleCreated(EditorID.Parse("?"), ToggleID.CreateNew(), "Test", "desc"));
			_session.LoadProjection<AllTogglesProjection>().Returns(projection);

			var response = await _handler.Handle(new GetAllTogglesRequest());

			response.Toggles.ShouldHaveSingleItem();
		}
	}
}
