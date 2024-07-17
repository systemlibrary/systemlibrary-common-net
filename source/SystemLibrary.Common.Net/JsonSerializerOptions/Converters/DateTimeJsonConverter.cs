using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

/// <summary>
/// Use to convert a string to DateTime during Json() invocation with your own format
/// </summary>
/// <remarks>
/// Class is exposed if you must specify your own date time format when Json() extension method could not convert it automatically
/// </remarks>
/// <example>
/// Example:
/// <code>
/// var options = new JsonSerializationOptions();
/// options.Converters.Add(new DateTimeJsonConverter("yyyy/MM/dd hh:mm"));
/// 
/// // Assume "json" is a string and somewhere there's a date on format "2000/12/24 12:30"
/// var result = json.Json(options);
/// </code>
/// </example>
public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    string Format;

    public DateTimeJsonConverter(string format = null)
    {
        Format = format;
    }

    public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString(Format));
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString().ToDateTime(Format);
    }
}
