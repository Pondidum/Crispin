using System.Text.RegularExpressions;
using Alba;

namespace Crispin.Rest.Tests.TestUtils
{
	public static class Extensions
	{
		public static Scenario HeaderShouldMatch(this Scenario scenario, string header, Regex regex)
		{
			return scenario.AssertThat(new RegexHeaderAssertion(header, regex));
		}
	}
}