using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrispinClient.Contexts
{
	public class ObjectContext : IToggleContext
	{
		private readonly object _target;

		public ObjectContext(object target)
		{
			_target = target;
		}

		public bool GroupContains(string groupName, string term)
		{
			var propertyFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase;

			var group = _target.GetType().GetProperty(groupName, propertyFlags);

			if (group == null)
				throw new MissingMemberException(_target.GetType().Name, groupName);

			var searchable = group.GetValue(_target) as IEnumerable<string>;

			if (searchable == null)
				throw new InvalidCastException($"Member '{_target.GetType().Name}.{group.Name}' does not implement IEnumerable<string>.");

			return searchable.Contains(term);
		}
	}
}
