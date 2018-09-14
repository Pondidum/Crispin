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
			var group = _target.GetType().GetProperty(groupName, BindingFlags.IgnoreCase);
			var searchable = group.GetValue(_target) as IEnumerable<string>;

			return searchable.Contains(term);
		}
	}
}
