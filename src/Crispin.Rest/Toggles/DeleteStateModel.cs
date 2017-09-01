using System.Collections.Generic;
using System.Linq;

namespace Crispin.Rest.Toggles
{
	public class DeleteStateModel
	{
		public IEnumerable<string> Groups { get; set; }
		public IEnumerable<string> Users { get; set; }

		public DeleteStateModel()
		{
			Groups = Enumerable.Empty<string>();
			Users = Enumerable.Empty<string>();
		}
	}
}
