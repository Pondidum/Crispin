using System.Collections.Generic;
using System.Linq;
using Crispin.Projections;
using Crispin.Views;

namespace Crispin.Handlers.GetAll
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
