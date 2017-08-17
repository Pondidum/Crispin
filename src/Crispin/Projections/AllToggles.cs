using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;

namespace Crispin.Projections
{
	public class AllToggles : Projection
	{
		public IEnumerable<KeyValuePair<Guid, string>> Toggles => _toggles;

		private readonly Dictionary<Guid, string> _toggles;

		public AllToggles()
		{
			_toggles = new Dictionary<Guid, string>();

			Register<ToggleCreated>(Apply);
		}

		private void Apply(ToggleCreated e) => _toggles[e.ID] = e.Name;
	}

	public class ToggleView
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }
		public IEnumerable<string> Tags { get; set; }

		public ToggleView()
		{
			Tags = Enumerable.Empty<string>();
		}
	}
}
