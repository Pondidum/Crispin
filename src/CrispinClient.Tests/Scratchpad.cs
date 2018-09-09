using System;
using System.Collections.Generic;
using System.Linq;
using Ruler;
using Ruler.Specifications;
using Xunit;

namespace CrispinClient.Tests
{
	public class Scratchpad
	{

		private class Toggles
		{
			public static readonly Guid Smars = Guid.NewGuid();
		}
		
		[Fact]
		public void When_testing_something()
		{
			IToggleQuery service = new ToggleService();
			
			var active = service.IsActive(Toggles.Smars, new
			{
				UserName = "testing"
			});
		}
	}


	public class ToggleService : IToggleQuery
	{
		private readonly Dictionary<Guid, Toggle> _toggles;

		public bool IsActive(Guid toggleID, object query)
		{
			if (_toggles.TryGetValue(toggleID, out var toggle) == false)
				throw new KeyNotFoundException(toggleID.ToString());

			return toggle.IsActive(new QueryAdapter(query));
		}

		public void Populate(IEnumerable<Toggle> toggles)
		{
			foreach (var toggle in toggles)
				_toggles[toggle.ID] = toggle;
		}
	}



	public interface IToggleQuery
	{
		bool IsActive(Guid toggleID, object query);
	}

	public class QueryAdapter : IActiveQuery
	{
		public QueryAdapter(object target)
		{
		}
	}
}
