using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

internal class StringJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetDouble(out double d))
                return d.ToString();

            if (reader.TryGetInt32(out int i))
                return i.ToString();

            if (reader.TryGetInt64(out long l))
                return l.ToString();

            return reader.GetUInt64().ToString();
        }
        
        if (reader.TokenType == JsonTokenType.True ||
            reader.TokenType == JsonTokenType.False ||
            reader.TokenType == JsonTokenType.String)
            return reader.GetString();

        throw new JsonException("StringJsonConverter cannot simply convert type: " + reader.TokenType);
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
