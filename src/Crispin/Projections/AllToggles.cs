using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Views;

namespace Crispin.Projections
{
	public class AllToggles : Projection<AllTogglesMemento>
	{
		public IEnumerable<ToggleView> Toggles => _toggles.Values;

		private readonly Dictionary<ToggleID, ToggleView> _toggles;

		public AllToggles()
		{
			_toggles = new Dictionary<ToggleID, ToggleView>();

			Register<ToggleCreated>(Apply);

			Register<TagAdded>(e => _toggles[e.AggregateID].Tags.Add(e.Name));
			Register<TagRemoved>(e => _toggles[e.AggregateID].Tags.Remove(e.Name));
		}

		private void Apply(ToggleCreated e) => _toggles.Add(e.ID, new ToggleView
		{
			ID = e.ID,
			Name = e.Name,
			Description = e.Description
		});

		protected override AllTogglesMemento CreateMemento()
		{
			return new AllTogglesMemento(_toggles.ToDictionary(p => p.Key.ToString(), p => p.Value));
		}

		protected override void ApplyMemento(AllTogglesMemento memento)
		{
			_toggles.Clear();

			foreach (var pair in memento)
				_toggles.Add(ToggleID.Parse(Guid.Parse(pair.Key)), pair.Value);
		}
	}

	public class AllTogglesMemento : Dictionary<string, ToggleView>
	{
		public AllTogglesMemento(IDictionary<string, ToggleView> other) : base(other)
		{
		}
	}
}
