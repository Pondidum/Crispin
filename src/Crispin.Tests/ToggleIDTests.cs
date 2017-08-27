using System;

namespace Crispin.Tests
{
	public class ToggleIDTests : IDTests<ToggleID>
	{
		protected override ToggleID CreateOne() => ToggleID.Parse(Guid.Parse("28FDBE6C-ACD5-4CEA-846A-894AB77C1365"));
		protected override ToggleID CreateTwo() => ToggleID.Parse(Guid.Parse("78BF2F7B-D2B2-4923-96C7-EFF231F8E6B2"));

		protected override ToggleID CreateNew() => ToggleID.CreateNew();
		protected override ToggleID Parse(Guid input) => ToggleID.Parse(input);
	}
}
