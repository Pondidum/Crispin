using System.Collections.Generic;
using Crispin.Projections;

namespace Crispin.Handlers
{
	public class GetAllTogglesResponse
	{
		public IEnumerable<ToggleView> Toggles { get; set; }
	}
}