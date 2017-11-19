using System.Collections.Generic;

namespace Crispin.Views
{
	public class StateView
	{
		public States Anonymous { get; set; }
		public Dictionary<UserID, States> Users { get; set; }
		public Dictionary<GroupID, States> Groups { get; set; }

		public StateView()
		{
			Anonymous = States.Off;
			Users = new Dictionary<UserID, States>();
			Groups = new Dictionary<GroupID, States>();
		}
	}
}
