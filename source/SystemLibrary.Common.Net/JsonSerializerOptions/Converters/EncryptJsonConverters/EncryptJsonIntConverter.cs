using System;
using System.Text.Json;

namespace SystemLibrary.Common.Net;

internal class EncryptJsonIntConverter : BaseEncryptJsonConverter<int>
{
    public EncryptJsonIntConverter(JsonEncryptAttribute attribute) : base(attribute)
    {
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return 0;

        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetDouble(out double doubleValue))
                return (int)doubleValue;

            if (reader.TryGetInt32(out int intValue))
                return intValue;
        }

        if (reader.TokenType == JsonTokenType.True)
            return 1;

        if (reader.TokenType == JsonTokenType.False)
            return 0;

        try
        {
            var value = Decrypt(ref reader);

            if (value.IsNot()) return 0;

            return int.Parse(value);
        }
        catch
        {
            if (reader.TryGetDouble(out double doubleValue))
                return (int)doubleValue;

            return reader.GetInt32();
        }
    }
}
