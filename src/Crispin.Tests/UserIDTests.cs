using System;

namespace Crispin.Tests
{
	public class UserIDTests : IDTests<UserID>
	{
		protected override UserID CreateNew() => UserID.Parse(Guid.NewGuid().ToString());
		protected override UserID Parse(Guid input) => UserID.Parse(input.ToString());
	}
}