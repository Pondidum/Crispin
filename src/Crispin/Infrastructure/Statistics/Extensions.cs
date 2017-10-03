using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Crispin.Infrastructure.Statistics
{
	public static class Extensions
	{
		private static readonly Regex Groups = new Regex(@"\{(.*?)\}");

		public static string Render(this string format, params object[] parameters)
		{
			var index = 0;

			return Groups.Replace(
				format,
				match =>
				{
					if (index >= parameters.Length)
						throw new FormatException("Not enough parameters were provided to render the string");

					return parameters[index++].ToString();
				});
		}

		public static IDictionary<string, object> BuildPropertyMap(this string template, params object[] parameters)
		{
			var index = 0;
			return Groups
				.Matches(template)
				.Cast<Match>()
				.ToDictionary(
					match => match.Groups[1].Value,
					match => parameters[index++]);
		}
	}
}
