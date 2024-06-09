using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

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
public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    string Format;

    public DateTimeOffsetJsonConverter(string format)
    {
        Format = format;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString(Format));
    }

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (value.IsNot())
            return DateTimeOffset.MinValue;

        if (DateTimeOffset.TryParse(value, out DateTimeOffset res))
            return res;

        if (DateTimeOffset.TryParse(value, null, System.Globalization.DateTimeStyles.AssumeUniversal, out res))
            return res;

        if (DateTimeOffset.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out res))
            return res;

        return DateTimeOffset.ParseExact(value, Format, null);
    }
}
