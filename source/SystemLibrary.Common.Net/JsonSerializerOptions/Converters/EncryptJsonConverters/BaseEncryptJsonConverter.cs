using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net;

internal class BaseEncryptJsonConverter<T> : JsonConverter<T>
{
    JsonEncryptAttribute Attribute;

    public BaseEncryptJsonConverter(JsonEncryptAttribute attribute)
    {
        this.Attribute = attribute;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var data = value.ToString();

        if (data.IsNot()) return;

        var encrypted = data.Encrypt(Attribute.Key, Attribute.IV, Attribute.AddedIV);

        writer.WriteStringValue(encrypted);
    }

    protected string Decrypt(ref Utf8JsonReader reader)
    {
        if (reader.TryGetBytesFromBase64(out byte[] b))
        {
            var base64 = b.ToBase64();

            return base64.Decrypt(Attribute.Key, Attribute.IV, Attribute.AddedIV);
        }
        return null;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
