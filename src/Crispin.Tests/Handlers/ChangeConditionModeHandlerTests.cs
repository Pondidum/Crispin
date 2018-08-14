using System.Threading;
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
		protected override ChangeConditionModeHandler CreateHandler(IStorageSession session)
		{
			return new ChangeConditionModeHandler(session);
		}

		[Fact]
		public async Task When_changing_to_enabled_on_any()
		{
			Toggle.EnableOnAllConditions(Editor);

			await Session.Save(Toggle);
			await Session.Commit();

			await Handler.Handle(new ChangeConditionModeRequest(Editor, Locator, ConditionModes.Any), CancellationToken.None);
			await Session.Commit();

			Event<EnabledOnAnyCondition>();
		}

		[Fact]
		public async Task When_changing_to_enabled_on_all()
		{
			Toggle.EnableOnAnyCondition(Editor);

			await Session.Save(Toggle);
			await Session.Commit();

			await Handler.Handle(new ChangeConditionModeRequest(Editor, Locator, ConditionModes.All), CancellationToken.None);
			await Session.Commit();

			Event<EnabledOnAllConditions>();
		}
	}
}
