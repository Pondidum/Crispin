using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Handlers.GetSingle;
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
		private readonly AllTogglesProjection _view;
		private readonly EditorID _creator;

		public GetToggleHandlerTests()
		{
			var storage = Substitute.For<IStorage>();
			var session = Substitute.For<IStorageSession>();
			storage.BeginSession().Returns(session);

			_view = new AllTogglesProjection();
			session.LoadProjection<AllTogglesProjection>().Returns(_view);

			_handler = new GetToggleHandler(storage);
			_creator = EditorID.Parse("test editor");
		}

		[Fact]
		public async Task When_the_requested_toggle_doesnt_exist_by_id()
		{
			var toggleID = ToggleID.CreateNew();

			var result = await _handler.Handle(new GetToggleRequest(ToggleLocator.Create(toggleID)));

			result.Toggle.ShouldBeNull();
		}

		[Fact]
		public async Task When_the_requested_toggle_exists_by_id()
		{
			var toggleID = ToggleID.CreateNew();
			_view.Consume(new ToggleCreated(_creator, toggleID, "name", "desc"));

			var result = await _handler.Handle(new GetToggleRequest(ToggleLocator.Create(toggleID)));

			result.Toggle.ID.ShouldBe(toggleID);
		}

		[Fact]
		public async Task When_the_requested_toggle_doesnt_exist_by_name()
		{
			var toggleName = ToggleID.CreateNew().ToString();

			var result = await _handler.Handle(new GetToggleRequest(ToggleLocator.Create(toggleName)));

			result.Toggle.ShouldBeNull();
		}

		[Fact]
		public async Task When_the_requested_toggle_exists_by_name()
		{
			var toggleName = "name";
			_view.Consume(new ToggleCreated(_creator,ToggleID.CreateNew(), toggleName, "desc"));

			var result = await _handler.Handle(new GetToggleRequest(ToggleLocator.Create(toggleName)));

			result.Toggle.Name.ShouldBe(toggleName);
		}
	}
}
