using System;

namespace Crispin.Tests
{
	public class EditorIDTests : IDTests<EditorID>
	{
		protected override EditorID CreateOne() => EditorID.Parse("one");
		protected override EditorID CreateTwo() => EditorID.Parse("two");

		protected override EditorID CreateNew() => EditorID.Parse(Guid.NewGuid().ToString());
		protected override EditorID Parse(Guid input) => EditorID.Parse(input.ToString());
	}
}
