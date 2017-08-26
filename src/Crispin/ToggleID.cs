using System;
using Newtonsoft.Json;

namespace Crispin
{
	[JsonConverter(typeof(ToggleIDConverter))]
	public class ToggleID : IEquatable<ToggleID>
	{
		public static ToggleID CreateNew() => new ToggleID(Guid.NewGuid());
		public static ToggleID Parse(Guid id) => new ToggleID(id);
		public static ToggleID Empty => new ToggleID(Guid.Empty);

		private readonly Guid _toggleID;

		private ToggleID(Guid toggleID)
		{
			_toggleID = toggleID;
		}

		public bool Equals(ToggleID other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return _toggleID.Equals(other._toggleID);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ToggleID)obj);
		}

		public override int GetHashCode() => _toggleID.GetHashCode();
		public override string ToString() => _toggleID.ToString();

		public static bool operator ==(ToggleID x, ToggleID y)
		{
			if (object.ReferenceEquals(x, y))
				return true;

			if (((object)x == null) || ((object)y == null))
				return false;

			return x.Equals(y);
		}

		public static bool operator !=(ToggleID x, ToggleID y)
		{
			return !(x == y);
		}
	}

	public class ToggleIDConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(ToggleID);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Guid guid;
			return Guid.TryParse(Convert.ToString(reader.Value), out guid)
				? ToggleID.Parse(guid)
				: null;
		}
	}
}
