using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrispinClient
{
	public class QueryAdapter : IActiveQuery
	{
		private readonly object _target;

		public QueryAdapter(object target)
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
