using System;
using System.Collections.Generic;

namespace CrispinClient
{
	public class ToggleService : IToggleQuery
	{
		private readonly Dictionary<Guid, Toggle> _toggles;

		public ToggleService()
		{
			_toggles = new Dictionary<Guid, Toggle>();
		}

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
}
