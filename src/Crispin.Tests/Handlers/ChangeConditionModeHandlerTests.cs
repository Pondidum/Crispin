using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Handlers.ChangeConditionMode;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class ChangeConditionModeHandlerTests : HandlerTest<ChangeConditionModeHandler>
	{
		protected override ChangeConditionModeHandler CreateHandler(IStorage storage)
		{
			return new ChangeConditionModeHandler(storage);
		}

		[Fact]
		public async Task When_changing_to_enabled_on_any()
		{
			Toggle.EnableOnAllConditions(Editor);

			using (var session = await Storage.BeginSession())
				await session.Save(Toggle);

			await Handler.Handle(new ChangeConditionModeRequest(Editor, Locator, ConditionModes.Any));

			Event<EnabledOnAnyCondition>();
		}

		[Fact]
		public async Task When_changing_to_enabled_on_all()
		{
			Toggle.EnableOnAnyCondition(Editor);

			using (var session = await Storage.BeginSession())
				await session.Save(Toggle);

			await Handler.Handle(new ChangeConditionModeRequest(Editor, Locator, ConditionModes.All));

			Event<EnabledOnAllConditions>();
		}
	}
}
