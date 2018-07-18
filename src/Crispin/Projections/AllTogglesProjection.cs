using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Views;

namespace Crispin.Projections
{
	public class AllTogglesProjection : Projection<AllTogglesMemento>
	{
		public IEnumerable<ToggleView> Toggles => _toggles.Values;

		private readonly Dictionary<ToggleID, ToggleView> _toggles;

		public AllTogglesProjection()
		{
			_toggles = new Dictionary<ToggleID, ToggleView>();
			var find = new Func<ToggleID, ToggleView>(id => _toggles[id]);

			Register<ToggleCreated>(e =>
			{
				var view = new ToggleView();
				view.Apply(e);
				_toggles.Add(e.AggregateID, view);
			});

			Register<TagAdded>(e => find(e.AggregateID).Apply(e));
			Register<TagRemoved>(e => find(e.AggregateID).Apply(e));
			Register<EnabledOnAllConditions>(e => find(e.AggregateID).Apply(e));
			Register<EnabledOnAnyCondition>(e => find(e.AggregateID).Apply(e));
			Register<ConditionAdded>(e => find(e.AggregateID).Apply(e));
			Register<ConditionRemoved>(e => find(e.AggregateID).Apply(e));
		}

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
