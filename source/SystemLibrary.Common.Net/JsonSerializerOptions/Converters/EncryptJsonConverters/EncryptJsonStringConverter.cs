using System;
using System.Text.Json;

namespace SystemLibrary.Common.Net;

internal class EncryptJsonStringConverter : BaseEncryptJsonConverter<string>
{
    public EncryptJsonStringConverter(JsonEncryptAttribute attribute) : base(attribute)
    {
    }

    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return Decrypt(ref reader);
        }
        catch
        {
            return reader.GetString();
        }
    }
}
