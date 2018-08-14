using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crispin.Handlers.GetAll;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
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
				.QueryProjection<ToggleView>()
				.Throws(new ProjectionNotRegisteredException("wat"));

			Should.Throw<ProjectionNotRegisteredException>(
				() => _handler.Handle(new GetAllTogglesRequest(), CancellationToken.None)
			);
		}
		
		[Fact]
		public async Task When_there_are_no_toggles_in_the_projection()
		{
			_session.QueryProjection<ToggleView>().Returns(new List<ToggleView>());

			var response = await _handler.Handle(new GetAllTogglesRequest(), CancellationToken.None);

			response.Toggles.ShouldBeEmpty();
		}

		[Fact]
		public async Task When_there_are_toggles_in_the_projection()
		{
			var projections = new List<ToggleView>
			{
				new ToggleView { ID = ToggleID.CreateNew(), Name = "Test", Description = "desc" }
			};

			_session.QueryProjection<ToggleView>().Returns(projections);

			var response = await _handler.Handle(new GetAllTogglesRequest(), CancellationToken.None);

			response.Toggles.ShouldHaveSingleItem();
		}
	}
}
