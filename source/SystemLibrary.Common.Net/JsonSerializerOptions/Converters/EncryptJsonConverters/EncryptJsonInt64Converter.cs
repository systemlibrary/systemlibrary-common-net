using System;
using System.Text.Json;

namespace SystemLibrary.Common.Net;

internal class EncryptJsonInt64Converter : BaseEncryptJsonConverter<long>
{
    public EncryptJsonInt64Converter(JsonEncryptAttribute attribute) : base(attribute)
    {
    }

    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return 0;

        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt64(out long value)) return value;

            return Convert.ToInt64(reader.GetDouble());
        }

        if (reader.TokenType == JsonTokenType.True)
            return 1;

        if (reader.TokenType == JsonTokenType.False)
            return 0;

        try
        {
            var value = Decrypt(ref reader);

            if (value.IsNot()) return 0L;

            return long.Parse(value);
        }
        catch
        {
            var v = reader.GetString();

            if (long.TryParse(v, out long f))
                return f;

            return Convert.ToInt64(double.Parse(v));
        }
    }
}
