using System.Collections.Generic;

namespace Crispin
{
	public interface IGroupMembership
	{
		IEnumerable<GroupID> GetGroupsFor(UserID userID);
	}
}
