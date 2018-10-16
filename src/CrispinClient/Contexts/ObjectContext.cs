using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CrispinClient.Contexts
{
	public class ObjectContext : IToggleContext
	{
		private const BindingFlags MemberFlags =
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

			var properties = FindPropertiesFor<IEnumerable<string>>(groupName);

			if (properties.Length == 0)
				throw new MissingMethodException(BuildMessage(groupName));

			var searchable = RunProperty<IEnumerable<string>>(properties.Single());

			if (searchable == null)
				throw new NullReferenceException($"Member '{_target.GetType().Name}.{properties.Single().Name}' is null.");

			return searchable.Contains(term);
		}

		public string GetCurrentUser()
		{
			var methods = FindMethodsFor<string>("GetCurrentUser")
				.Where(m => m.GetParameters().Length == 0)
				.ToArray();

			if (methods.Length == 1)
				return RunMethod<string>(methods.Single());

			var properties = FindPropertiesFor<string>("CurrentUser");

			if (properties.Length == 0)
				return string.Empty;

			return RunProperty<string>(properties.First());
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

		private PropertyInfo[] FindPropertiesFor<TReturn>(string groupName) => _target
			.GetType()
			.GetProperties(MemberFlags)
			.Where(prop => prop.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase))
			.Where(prop => typeof(TReturn).IsAssignableFrom(prop.PropertyType))
			.Where(prop => prop.GetIndexParameters().Length == 0)
			.ToArray();

		private IEnumerable<MethodInfo> FindMethodsFor<TReturn>(string groupName) => _target
			.GetType()
			.GetMethods(MemberFlags)
			.Where(m => m.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase))
			.Where(m => m.ReturnType == typeof(TReturn));

		private TReturn RunProperty<TReturn>(PropertyInfo property) => (TReturn)property.GetValue(_target);
		private TReturn RunMethod<TReturn>(MethodInfo method, params object[] args) => (TReturn)method.Invoke(_target, args);
	}
}
