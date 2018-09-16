using System;
using System.Collections.Generic;
using CrispinClient.Contexts;

namespace CrispinClient
{
	public class ToggleService : IToggleQuery
	{
		private readonly Dictionary<Guid, Toggle> _toggles;

		public ToggleService()
		{
			_toggles = new Dictionary<Guid, Toggle>();
		}

		public bool IsActive(Guid toggleID, object context) => IsActive(toggleID, new ObjectContext(context));

		public bool IsActive(Guid toggleID, IToggleContext context)
		{
			if (_toggles.TryGetValue(toggleID, out var toggle) == false)
				throw new KeyNotFoundException(toggleID.ToString());

			return toggle.IsActive(context);
		}

		public void Populate(IEnumerable<Toggle> toggles)
		{
			foreach (var toggle in toggles)
				_toggles[toggle.ID] = toggle;
		}
	}
}
