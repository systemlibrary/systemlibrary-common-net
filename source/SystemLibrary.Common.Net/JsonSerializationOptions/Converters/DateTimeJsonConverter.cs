using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net
{
	/// <summary>
	/// DateTime json converter that takes a string format
	/// </summary>
	/// <example>
	/// Example of an additional datetime converter:
	/// <code>
	/// var options = new JsonSerializationOptions();
	/// options.Converters.Add(new DateTimeJsonConverter("yyyy/MM/dd hh:mm"));
	/// </code>
	/// </example>
	public class DateTimeJsonConverter : JsonConverter<DateTime>
	{
		string Format;

		public DateTimeJsonConverter(string format)
		{
			Format = format;
		}

		public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
		{
			writer.WriteStringValue(date.ToString(Format));
		}

		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var value = reader.GetString();

			if (value.IsNot())
				return DateTime.MinValue;

			if (DateTime.TryParse(value, out DateTime res))
				return res;

			if (DateTime.TryParseExact(value, Format, null, System.Globalization.DateTimeStyles.AssumeUniversal, out res))
				return res;

			if (DateTime.TryParseExact(value, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.RoundtripKind, out res))
				return res;

			return DateTime.ParseExact(value, Format, null);
		}
	}
}
