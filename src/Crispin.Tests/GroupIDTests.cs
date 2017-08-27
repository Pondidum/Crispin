using System;

namespace Crispin.Tests
{
	public class GroupIDTests : IDTests<GroupID>
	{
		protected override GroupID CreateNew() => GroupID.Parse(Guid.NewGuid().ToString());
		protected override GroupID Parse(Guid input) => GroupID.Parse(input.ToString());
	}
}
