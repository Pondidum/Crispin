namespace CrispinClient
{
	public interface IActiveQuery
	{
		bool GroupContains(string groupName, string term);
	}
}
