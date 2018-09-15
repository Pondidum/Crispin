using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CrispinClient.Contexts
{
	public class ObjectContext : IToggleContext
	{
		private const BindingFlags PropertyFlags =
			BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Instance |
			BindingFlags.IgnoreCase;

		private readonly object _target;

		public ObjectContext(object target)
		{
			_target = target;
		}

		public bool GroupContains(string groupName, string term)
		{
			var methods = _target.GetType()
				.GetMethods(PropertyFlags)
				.Where(m => m.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase))
				.ToArray();

			if (methods.Any())
				return RunMethod(methods, groupName, term);

			var property = _target.GetType().GetProperty(groupName, PropertyFlags);

			return RunProperty(property, groupName, term);
		}

		private bool RunProperty(PropertyInfo property, string groupName, string term)
		{
			if (property == null)
				throw new MissingMemberException(_target.GetType().Name, groupName);

			if (typeof(IEnumerable<string>).IsAssignableFrom(property.PropertyType) == false)
				throw new InvalidCastException($"Member '{_target.GetType().Name}.{property.Name}' does not implement IEnumerable<string>.");

			var searchable = (IEnumerable<string>)property.GetValue(_target);

			if (searchable == null)
				throw new NullReferenceException($"Member '{_target.GetType().Name}.{property.Name}' is null.");

			return searchable.Contains(term);
		}

		private bool RunMethod(MethodInfo[] methods, string groupName, string term)
		{
			var matches = methods
				.Where(m => m.ReturnType == typeof(bool))
				.Where(m => m.GetParameters().Length == 1)
				.Where(m => m.GetParameters().Single().ParameterType == typeof(string))
				.ToArray();

			if (matches.Length != 1)
			{
				var message = $"No method found with a signature 'bool {_target.GetType().Name}.{groupName}(string term)' (case insensitive).";

				if (methods.Length > 1)
					message = message + BuildSignatures(methods);

				throw new MissingMethodException(message);
			}

			return (bool)matches.Single().Invoke(_target, new object[] { term });
		}

		private string BuildSignatures(MethodInfo[] methods)
		{
			var targetName = _target.GetType().Name;
			
			var sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine("Found:");

			foreach (var method in methods)
			{
				var parameters = method.GetParameters().Select(m => $"{m.ParameterType.Name} {m.Name}");
				
				sb.AppendLine($"* {method.ReturnType.Name} {targetName}.{method.Name}({string.Join(", ", parameters)})");
			}

			return sb.ToString();
		}
	}
}
