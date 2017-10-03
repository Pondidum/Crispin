using System;
using System.Text.RegularExpressions;

namespace Crispin.Infrastructure.Statistics
{
	public static class Extensions
	{
		public static string Render(this string format, params object[] parameters)
		{
			var rx = new Regex(@"\{(.*?)\}");
			var index = 0;

			return rx.Replace(
				format,
				match =>
				{
					if (index >= parameters.Length)
						throw new FormatException("Not enough parameters were provided to render the string");

					return parameters[index++].ToString();
				});
		}
	}
}
