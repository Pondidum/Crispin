using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

		public void SwitchOnByDefault() => _states.HandleSwitching(true);
		public void SwitchOffByDefault() => _states.HandleSwitching(false);

		public void SwitchOn(UserID user) => _states.HandleSwitching(user, true);
		public void SwitchOff(UserID user) => _states.HandleSwitching(user, false);

		public void SwitchOn(GroupID group) => _states.HandleSwitching(group, true);
		public void SwitchOff(GroupID group) => _states.HandleSwitching(group, false);
	}
}
