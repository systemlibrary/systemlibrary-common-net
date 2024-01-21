using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net
{
    internal class LongJsonConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return 0;

            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt64();

            if (reader.TokenType == JsonTokenType.True)
                return 1;

            if (reader.TokenType == JsonTokenType.False)
                return 0;

            if (reader.TokenType == JsonTokenType.String)
                return long.Parse(reader.GetString());

            throw new JsonException("Error reading: " + reader.GetString() + " into an Int64/long");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    // Creds: https://stackoverflow.com/questions/66919668/net-core-graphql-graphql-systemtextjson-serialization-and-deserialization-of
    internal class TypeConverter : JsonConverter<Type>
    {
        public override Type Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
            )
        {
            var assemblyQualifiedName = reader.GetString();

            if (assemblyQualifiedName == null) return default;

            return Type.GetType(assemblyQualifiedName);
        }

        public override void Write(
            Utf8JsonWriter writer,
            Type value,
            JsonSerializerOptions options
            )
        {
            writer.WriteStringValue(value?.AssemblyQualifiedName);
        }
    }
}
