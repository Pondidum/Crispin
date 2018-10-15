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
			var methods = FindMethodsFor<bool>(groupName)
				.Where(m => m.GetParameters().Length == 1)
				.Where(m => m.GetParameters().Single().ParameterType == typeof(string))
				.ToArray();

			if (methods.Length == 1)
				return RunMethod<bool>(methods.Single(), term);

			var property = FindProperty(groupName);

			if (property != null)
				return RunProperty(property, term);

			throw new MissingMethodException(BuildMessage(groupName));
		}

		public string GetCurrentUser()
		{
			var methods = FindMethodsFor<string>("GetCurrentUser")
				.Where(m => m.GetParameters().Length == 0)
				.ToArray();

			if (methods.Length == 1)
				return RunMethod<string>(methods.Single());

			var property = _target.GetType().GetProperty("CurrentUser", PropertyFlags);

			if (property == null)
				return string.Empty;

			var value = property.GetValue(_target);

			return Convert.ToString(value);
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

		private MethodInfo[] FindMethodsFor<TReturn>(string groupName) => _target
			.GetType()
			.GetMethods(PropertyFlags)
			.Where(m => m.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase))
			.Where(m => m.ReturnType == typeof(TReturn))
			.ToArray();

		private TReturn RunMethod<TReturn>(MethodInfo method, params object[] args)
		{
			return (TReturn)method.Invoke(_target, args);
		}
	}
}
