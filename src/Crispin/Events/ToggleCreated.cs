namespace Crispin.Events
{
	public class ToggleCreated
	{
		public ToggleID NewToggleID { get; }
		public EditorID Creator { get; }
		public string Name { get; }
		public string Description { get; }
		public ConditionModes ConditionMode { get; }

		public ToggleCreated(EditorID creator, ToggleID newToggleID, string name, string description, ConditionModes conditionMode)
		{
			NewToggleID = newToggleID;
			Creator = creator;
			Name = name;
			Description = description;
			ConditionMode = conditionMode;
		}

		public override string ToString()
		{
			return $"Creating Toggle called '{Name}' with description '{Description}'";
		}
	}
}
