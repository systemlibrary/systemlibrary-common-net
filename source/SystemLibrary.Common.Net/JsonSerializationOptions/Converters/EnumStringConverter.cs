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
            {
                if (reader.TryGetInt32(out int i32))
                    return (TEnum)(object)i32;

                if (reader.TryGetInt64(out long i64))
                    return (TEnum)(object)i64;
            }


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
                    // NOTE: Fallback to ToEnum, reading attributes, not efficient
                    return (TEnum)value.ToEnum(Type);
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
