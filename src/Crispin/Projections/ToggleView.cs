using System;
using System.Collections.Generic;

namespace Crispin.Projections
{
	public class ToggleView
	{
		public ToggleID ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HashSet<string> Tags { get; set; }
		public StateView State { get; set; }

		private readonly ToggleState _states;

		public ToggleView()
		{
			Tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			State = new StateView();
			_states = new ToggleState(State);
		}

		public void SwitchOnByDefault() => _states.HandleSwitching(States.On);
		public void SwitchOffByDefault() => _states.HandleSwitching(States.Off);

		public void SwitchOn(UserID user) => _states.HandleSwitching(user, States.On);
		public void SwitchOff(UserID user) => _states.HandleSwitching(user, States.Off);

		public void SwitchOn(GroupID group) => _states.HandleSwitching(group, States.On);
		public void SwitchOff(GroupID group) => _states.HandleSwitching(group, States.Off);
	}
}
