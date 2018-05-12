using System;
using System.Collections.Generic;

namespace Crispin.Views
{
	public class ToggleView
	{
		public ToggleID ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HashSet<string> Tags { get; set; }

		public ToggleView()
		{
			Tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}
	}
}
