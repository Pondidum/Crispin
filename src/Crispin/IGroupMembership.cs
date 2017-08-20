using System.Collections.Generic;

namespace Crispin
{
	public interface IGroupMembership
	{
		IEnumerable<string> GetGroupsFor(string userID);
	}
}
