using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class GetToggleHandlerTests
	{
		private readonly GetToggleHandler _handler;
		private readonly List<ToggleView> _view;
		private readonly EditorID _creator;

		public GetToggleHandlerTests()
		{
			_view = new List<ToggleView>();

			var session = Substitute.For<IStorageSession>();
			session.QueryProjection<ToggleView>().Returns(_view);

			_handler = new GetToggleHandler(session);
			_creator = EditorID.Parse("test editor");
		}

		[Fact]
		public async Task When_the_requested_toggle_doesnt_exist_by_id()
		{
			var toggleID = ToggleID.CreateNew();

			var result = await _handler.Handle(new GetToggleRequest(ToggleLocator.Create(toggleID)), CancellationToken.None);

			result.Toggle.ShouldBeNull();
		}

		[Fact]
		public async Task When_the_requested_toggle_exists_by_id()
		{
			var toggleID = ToggleID.CreateNew();
			_view.Add(new ToggleView { ID = toggleID, Name = "name", Description = "desc" });

			var result = await _handler.Handle(new GetToggleRequest(ToggleLocator.Create(toggleID)), CancellationToken.None);

			result.Toggle.ID.ShouldBe(toggleID);
		}

		[Fact]
		public async Task When_the_requested_toggle_doesnt_exist_by_name()
		{
			var toggleName = ToggleID.CreateNew().ToString();

			var result = await _handler.Handle(new GetToggleRequest(ToggleLocator.Create(toggleName)), CancellationToken.None);

			result.Toggle.ShouldBeNull();
		}

		[Fact]
		public async Task When_the_requested_toggle_exists_by_name()
		{
			var toggleName = "name";
			_view.Add(new ToggleView { ID = ToggleID.CreateNew(), Name = toggleName, Description = "desc" });

			var result = await _handler.Handle(new GetToggleRequest(ToggleLocator.Create(toggleName)), CancellationToken.None);

			result.Toggle.Name.ShouldBe(toggleName);
		}
	}
}
