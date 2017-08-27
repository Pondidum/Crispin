using System;
using System.Collections.Generic;
using Crispin.Events;
using Crispin.Infrastructure;

namespace Crispin.Projections
{
	public class AllToggles : Projection
	{
		public IEnumerable<ToggleView> Toggles => _toggles.Values;

		private readonly Dictionary<ToggleID, ToggleView> _toggles;

		public AllToggles()
		{
			_toggles = new Dictionary<ToggleID, ToggleView>();

			Register<ToggleCreated>(Apply);

			Register<ToggleSwitchedOnForAnonymous>(e => _toggles[e.AggregateID].SwitchOnByDefault());
			Register<ToggleSwitchedOffForAnonymous>(e => _toggles[e.AggregateID].SwitchOffByDefault());
			Register<ToggleSwitchedOnForUser>(e => _toggles[e.AggregateID].SwitchOn(e.User));
			Register<ToggleSwitchedOffForUser>(e => _toggles[e.AggregateID].SwitchOff(e.User));
			Register<ToggleSwitchedOnForGroup>(e => _toggles[e.AggregateID].SwitchOn(e.Group));
			Register<ToggleSwitchedOffForGroup>(e => _toggles[e.AggregateID].SwitchOff(e.Group));

			Register<TagAdded>(e => _toggles[e.AggregateID].Tags.Add(e.Name));
			Register<TagRemoved>(e => _toggles[e.AggregateID].Tags.Remove(e.Name));
		}

		private void Apply(ToggleCreated e) => _toggles.Add(e.ID, new ToggleView
		{
			ID = e.ID,
			Name = e.Name,
			Description = e.Description
		});
	}
}
