namespace Crispin.Conditions
{
	public abstract class Condition
	{
		public string ConditionType => GetType().Name.Replace("Condition", "");
		public ConditionID ID { get; set; }
	}
}
