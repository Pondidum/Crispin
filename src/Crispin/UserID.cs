using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Crispin
{
	[TypeConverter(typeof(UserIDTypeConverter))]
	[JsonConverter(typeof(UserIDConverter))]
	public struct UserID : IEquatable<UserID>
	{
		public static UserID Parse(string user) => new UserID(user);
		public static UserID Empty => new UserID(string.Empty);

		private readonly string _user;

		private UserID(string user)
		{
			_user = string.IsNullOrWhiteSpace(user)
				? null
				: user;
		}

		public bool Equals(UserID other)
		{
			return string.Equals(_user, other._user, StringComparison.OrdinalIgnoreCase);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is UserID && Equals((UserID)obj);
		}

		public override int GetHashCode() => _user != null
			? StringComparer.OrdinalIgnoreCase.GetHashCode(_user)
			: 0;

		public override string ToString() => _user;

		public static bool operator ==(UserID x, UserID y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(UserID x, UserID y)
		{
			return !(x == y);
		}
	}

	public class UserIDConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(UserID);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return UserID.Parse(Convert.ToString(reader.Value) ?? string.Empty);
		}
	}

	public class UserIDTypeConverter : IDTypeConverter
	{
		protected override object Parse(string value) => UserID.Parse(value);
	}
}
