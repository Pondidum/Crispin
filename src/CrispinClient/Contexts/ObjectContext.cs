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
			var method = FindMethod(groupName, term);

			if (method != null)
				return RunMethod(method, term);

			var property = FindProperty(groupName);

			if (property != null)
				return RunProperty(property, term);

			throw new MissingMethodException(BuildMessage(groupName));
		}

		private string BuildMessage(string groupName)
		{
			var sb = new StringBuilder();

			sb.AppendLine($"No method or property found to query for group '{groupName}'.");
			sb.AppendLine("Supported method/property signatures are (case insensitive):");
			sb.AppendLine();
			sb.AppendLine($"* 'bool {_target.GetType().Name}.{groupName}(string term)'");
			sb.AppendLine($"* 'IEnumerable<string> {_target.GetType().Name}.{groupName} {{ get; }}'");
			sb.AppendLine();
			sb.AppendLine("Properties can be of any type which implements IEnumerable<string>.");

			return sb.ToString();
		}

		private PropertyInfo FindProperty(string groupName)
		{
			var property = _target.GetType().GetProperty(groupName, PropertyFlags);

			if (property == null)
				return null;

			if (typeof(IEnumerable<string>).IsAssignableFrom(property.PropertyType) == false)
				throw new InvalidCastException($"Member '{_target.GetType().Name}.{property.Name}' does not implement IEnumerable<string>.");

			return property;
		}

		private bool RunProperty(PropertyInfo property, string term)
		{
			var searchable = (IEnumerable<string>)property.GetValue(_target);

			if (searchable == null)
				throw new NullReferenceException($"Member '{_target.GetType().Name}.{property.Name}' is null.");

			return searchable.Contains(term);
		}

		private MethodInfo FindMethod(string groupName, string term)
		{
			var methods = _target.GetType()
				.GetMethods(PropertyFlags)
				.Where(m => m.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase))
				.ToArray();

			if (methods.Any() == false)
				return null;

			var matches = methods
				.Where(m => m.ReturnType == typeof(bool))
				.Where(m => m.GetParameters().Length == 1)
				.Where(m => m.GetParameters().Single().ParameterType == typeof(string))
				.ToArray();

			if (matches.Length == 1)
				return matches.Single();

			return null;
		}

		private bool RunMethod(MethodInfo method, string term)
		{
			return (bool)method.Invoke(_target, new object[] { term });
		}
	}
}
