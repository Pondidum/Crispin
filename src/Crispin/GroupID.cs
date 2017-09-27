using System;
using Newtonsoft.Json;

namespace Crispin
{
	[JsonConverter(typeof(GroupIDConverter))]
	public struct GroupID : IEquatable<GroupID>
	{
		public static GroupID Parse(string user) => new GroupID(user);
		public static GroupID Empty => new GroupID(string.Empty);

		private readonly string _group;

		private GroupID(string group)
		{
			_group = string.IsNullOrWhiteSpace(group)
				? null
				: group;
		}

		public bool Equals(GroupID other)
		{
			return string.Equals(_group, other._group, StringComparison.OrdinalIgnoreCase);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is GroupID && Equals((GroupID)obj);
		}

		public override int GetHashCode() => _group != null
			? StringComparer.OrdinalIgnoreCase.GetHashCode(_group)
			: 0;

		public override string ToString() => _group;

		public static bool operator ==(GroupID x, GroupID y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(GroupID x, GroupID y)
		{
			return !(x == y);
		}
	}

	public class GroupIDConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(GroupID);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return GroupID.Parse(Convert.ToString(reader.Value) ?? string.Empty);
		}
	}
}
