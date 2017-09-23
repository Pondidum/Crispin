namespace Crispin.Rest.Toggles
{
	public class TogglePostRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
	}

	public class StatePutRequest
	{
		public States State { get; set; }
	}
}
