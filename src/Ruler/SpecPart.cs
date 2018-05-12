using System.Collections.Generic;
using System.Linq;

namespace Ruler
{
	public class SpecPart
	{
		public string Type { get; set; }
		public IEnumerable<SpecPart> Children { get; set; }

		public SpecPart()
		{
			Children = Enumerable.Empty<SpecPart>();
		}
	}
}
