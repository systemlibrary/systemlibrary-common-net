using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net
{
    internal class EnumStringConverter<TEnum> : JsonConverter<TEnum> where TEnum : IComparable, IFormattable, IConvertible
    {
        static Type Type;

        public EnumStringConverter()
        {
            Type = typeof(TEnum);
        }
        
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
                return (TEnum)(object)reader.GetInt32();

            if (reader.TokenType == JsonTokenType.Null || reader.TokenType == JsonTokenType.False)
                return default;

            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (value.IsNot())
                    return default;

                if (!Enum.TryParse(Type, value, false, out var result)
                    && !Enum.TryParse(Type, value, true, out result))
                {
                    //TODO: Consider reading "attributes conversion" like "ToEnum<>()" does
                    throw new JsonException("Could not convert " + value + " to Enum " + Type.Name);
                }
                return (TEnum)result;
            }
            throw new JsonException("Could not convert token-type " + reader.TokenType + " to Enum " + Type.Name);
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }
    }
}
