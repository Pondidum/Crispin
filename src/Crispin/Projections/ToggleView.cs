using System;
using System.Collections.Generic;
using System.Linq;

namespace Crispin.Projections
{
	public class ToggleView
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }
		public HashSet<string> Tags { get; set; }

		public ToggleView()
		{
			Tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}
	}
}
