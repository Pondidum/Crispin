using System;

namespace Crispin.Tests
{
	public class ToggleIDTests : IDTests<ToggleID>
	{
		protected override ToggleID CreateNew() => ToggleID.CreateNew();
		protected override ToggleID Parse(Guid input) => ToggleID.Parse(input);
	}
}
