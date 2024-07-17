using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

internal class IntJsonConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return 0;

        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetDouble(out double doubleValue))
                return (int)doubleValue;

            throw new Exception("Cannot convert value to int32");
        }

        if (reader.TokenType == JsonTokenType.True)
            return 1;

        if (reader.TokenType == JsonTokenType.False)
            return 0;

        if (reader.TokenType == JsonTokenType.String)
            return int.Parse(reader.GetString());

        throw new JsonException("Error reading: " + reader.GetString() + " into an Int32");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
