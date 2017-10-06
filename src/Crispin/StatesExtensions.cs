namespace Crispin
{
	public static class StatesExtensions
	{
		public static States? AsState(this bool value) => value ? States.On : States.Off;
		public static States? AsState(this bool? value) => value?.AsState();

		public static string Render(this States? state) => state.HasValue
			? state.Value.ToString()
			: "unset";
	}
}
