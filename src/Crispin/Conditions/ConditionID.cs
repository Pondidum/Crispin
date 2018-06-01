using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace Crispin.Conditions
{
	[TypeConverter(typeof(ConditionIDTypeConverter))]
	[JsonConverter(typeof(ConditionIDConverter))]
	public class ConditionID : IEquatable<ConditionID>
	{
		public static ConditionID Parse(int id) => new ConditionID(id);
		public static ConditionID Empty => new ConditionID(-1);

		private readonly int _id;

		private ConditionID(int toggleID)
		{
			_id = toggleID;
		}

		[Pure]
		public ConditionID Next() => new ConditionID(_id + 1);

		public bool IsNewerThan(ConditionID other) => _id > other._id;

		public bool Equals(ConditionID other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return _id.Equals(other._id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ConditionID)obj);
		}

		public override int GetHashCode() => _id.GetHashCode();
		public override string ToString() => _id.ToString();

		public static bool operator ==(ConditionID x, ConditionID y)
		{
			if (object.ReferenceEquals(x, y))
				return true;

			if (((object)x == null) || ((object)y == null))
				return false;

			return x.Equals(y);
		}

		public static bool operator !=(ConditionID x, ConditionID y)
		{
			return !(x == y);
		}
	}

	public class ConditionIDConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(ConditionID);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(Convert.ToInt32(value.ToString()));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			int id;
			return int.TryParse(Convert.ToString(reader.Value), out id)
				? ConditionID.Parse(id)
				: null;
		}
	}

	public class ConditionIDTypeConverter : IDTypeConverter
	{
		protected override object Parse(string value) => ConditionID.Parse(Convert.ToInt32(value));
	}
}
