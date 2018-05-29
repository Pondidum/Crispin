using System;
using Crispin.Conditions;

namespace Crispin.Tests.Conditions
{
	public class ConditionIDTests : IDTests<ConditionID, int>
	{
		private static readonly Random Random = new Random();

		protected override ConditionID CreateOne() => ConditionID.Parse(1234);
		protected override ConditionID CreateTwo() => ConditionID.Parse(6789);

		protected override ConditionID CreateNew() => ConditionID.Parse(Random.Next(0, 10000));
		protected override ConditionID Parse(int input)=> ConditionID.Parse(input);

		protected override int GenerateRandomID() => Random.Next(0, 10000);

		protected override string JsonValue(int wrapped)
		{
			return wrapped.ToString();
		}
	}
}
