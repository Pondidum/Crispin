namespace Crispin.Conditions
{
	public class ConditionIDGenerator
	{
		private int _id;

		public ConditionIDGenerator(int first)
		{
			_id = first;
		}

		public ConditionID Next() => ConditionID.Parse(_id++);
	}
}
