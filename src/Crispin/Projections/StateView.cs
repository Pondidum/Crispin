using System.Collections.Generic;

namespace Crispin.Projections
{
	public class StateView
	{
		public bool Anonymous { get; set; }
		public Dictionary<string, bool> Users { get; set; }
		public Dictionary<string, bool> Groups { get; set; }
	}
}
