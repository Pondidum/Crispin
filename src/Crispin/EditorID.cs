using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Crispin
{
	[TypeConverter(typeof(EditorIDTypeConverter))]
	[JsonConverter(typeof(EditorIDConverter))]
	public struct EditorID : IEquatable<EditorID>
	{
		public static EditorID Parse(string user) => new EditorID(user);

		private readonly string _user;

		private EditorID(string user)
		{
			if (string.IsNullOrWhiteSpace(user))
				throw new ArgumentNullException(nameof(user));

			_user = user;
		}

		public bool Equals(EditorID other)
		{
			return string.Equals(_user, other._user, StringComparison.OrdinalIgnoreCase);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is EditorID && Equals((EditorID)obj);
		}

		public override int GetHashCode() => _user != null
			? StringComparer.OrdinalIgnoreCase.GetHashCode(_user)
			: 0;

		public override string ToString() => _user;

		public static bool operator ==(EditorID x, EditorID y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(EditorID x, EditorID y)
		{
			return !(x == y);
		}
	}

	public class EditorIDConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(EditorID);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return EditorID.Parse(Convert.ToString(reader.Value) ?? string.Empty);
		}
	}

	public class EditorIDTypeConverter : IDTypeConverter
	{
		protected override object Parse(string value) => EditorID.Parse(value);
	}
}
