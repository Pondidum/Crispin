using System.Collections.Generic;

namespace Crispin.Rest.Toggles
{
	public class DeleteStateModel
	{
		public IEnumerable<string> Groups { get; set; }
		public IEnumerable<string> Users { get; set; }
	}
}
