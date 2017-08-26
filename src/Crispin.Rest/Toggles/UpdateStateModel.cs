using System.Collections.Generic;

namespace Crispin.Rest.Toggles
{
	public class UpdateStateModel
	{
		public bool? Anonymous { get; set; }
		public Dictionary<string, bool> Users { get; set; }
		public Dictionary<string, bool> Groups { get; set; }
	}
}
