using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
			var group = _target.GetType().GetProperty(groupName, PropertyFlags);

			if (group == null)
				throw new MissingMemberException(_target.GetType().Name, groupName);

			if (typeof(IEnumerable<string>).IsAssignableFrom(group.PropertyType) == false)
				throw new InvalidCastException($"Member '{_target.GetType().Name}.{group.Name}' does not implement IEnumerable<string>.");

			var searchable = (IEnumerable<string>)group.GetValue(_target);

			if (searchable == null)
				throw new NullReferenceException($"Member '{_target.GetType().Name}.{group.Name}' is null.");

			return searchable.Contains(term);
		}
	}
}
