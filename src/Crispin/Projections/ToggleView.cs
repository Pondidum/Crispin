using System;
using System.Collections.Generic;
using System.Linq;

namespace Crispin.Projections
{
	public class ToggleView
	{
		public ToggleID ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HashSet<string> Tags { get; }
		public StateView State => new StateView
		{
			Anonymous = _states.AnonymousState,
			Users = _states.UserState,
			Groups = _states.GroupState
		};

		private readonly ToggleState _states;
		public ToggleView()
		{
			Tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			_states = new ToggleState();
		}

		public void SwitchOn(string user, string group) => _states.HandleSwitching(user, group, true);
		public void SwitchOff(string user, string group) => _states.HandleSwitching(user, group, false);
	}
}
