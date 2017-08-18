using System.Collections.Generic;
using System.Linq;
using Crispin.Projections;

namespace Crispin.Handlers
{
	public class GetAllTogglesResponse
	{
		public IEnumerable<ToggleView> Toggles { get; set; }

		public GetAllTogglesResponse()
		{
			Toggles = Enumerable.Empty<ToggleView>();
		}
	}
}
