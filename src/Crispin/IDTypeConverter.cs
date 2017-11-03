using System;
using System.ComponentModel;
using System.Globalization;

namespace Crispin
{
	public abstract class IDTypeConverter : TypeConverter
	{
		protected abstract object Parse(string value);

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string == false)
				return base.ConvertFrom(context, culture, value);

			return Parse((string)value);
		}
	}
}
