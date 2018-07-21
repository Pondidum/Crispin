using System;
using System.Collections.Generic;
using System.Text;

namespace Crispin.Infrastructure.Storage
{
	public class BuilderNotFoundException : Exception
	{
		public BuilderNotFoundException(IEnumerable<Type> availableBuilders, Type aggregate)
			: base(BuildMessage(availableBuilders, aggregate))
		{
		}

		private static string BuildMessage(IEnumerable<Type> availableBuilders, Type aggregate)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"No builder for type {aggregate.Name} found.");
			sb.AppendLine($"Did you forget to call Storage.{nameof(IStorage.RegisterAggregate)}()?");
			sb.AppendLine();

			sb.Append("Builders for the following types were registered:");

			foreach (var builder in availableBuilders)
				sb.AppendLine(builder.Name);

			return sb.ToString();
		}
	}
}
