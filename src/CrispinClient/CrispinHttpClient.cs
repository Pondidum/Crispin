using System;
using System.Collections.Generic;
using CrispinClient.Conditions;

namespace CrispinClient
{
	public class CrispinHttpClient
	{
		private bool _enabled;

		public IEnumerable<Toggle> GetAllToggles()
		{
			_enabled = !_enabled;
			yield return new Toggle
			{
				ID = Guid.NewGuid(),
				Conditions = _enabled
					? new Condition[] { new EnabledCondition() }
					: new Condition[] { new DisabledCondition() }
			};
		}
	}
}
