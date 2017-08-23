using System;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Handlers;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class GetToggleHandlerTests
	{
		private readonly GetToggleHandler _handler;
		private readonly AllToggles _view;

		public GetToggleHandlerTests()
		{
			var storage = Substitute.For<IStorage>();
			var session = Substitute.For<IStorageSession>();
			storage.BeginSession().Returns(session);

			_view = new AllToggles();
			session.LoadProjection<AllToggles>().Returns(_view);

			_handler = new GetToggleHandler(storage);
		}

		[Fact]
		public async Task When_the_requested_toggle_doesnt_exist()
		{
			var toggleID = Guid.NewGuid();

			var result = await _handler.Handle(new GetToggleRequest(toggleID));

			result.Toggle.ShouldBeNull();
		}

		[Fact]
		public async Task When_the_requested_toggle_exists()
		{
			var toggleID = Guid.NewGuid();
			_view.Consume(new ToggleCreated(toggleID, "name", "desc"));

			var result = await _handler.Handle(new GetToggleRequest(toggleID));

			result.Toggle.ID.ShouldBe(toggleID);
		}
	}
}
