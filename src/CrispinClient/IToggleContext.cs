namespace CrispinClient
{
	public interface IToggleContext
	{
		string GetCurrentUser();
		bool GroupContains(string groupName, string term);
	}
}
