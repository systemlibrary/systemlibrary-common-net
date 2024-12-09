using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net;

internal class BaseObfuscateJsonConverter<T> : JsonConverter<T>
{
    JsonObfuscateAttribute Attribute;

    public BaseObfuscateJsonConverter(JsonObfuscateAttribute attribute)
    {
        Attribute = attribute;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null) return;

        var data = value.ToString();

        writer.WriteStringValue(data.Obfuscate(Attribute.Salt).ToBase64());
    }

    protected string Deobfuscate(ref Utf8JsonReader reader)
    {
        if (reader.TryGetBytesFromBase64(out byte[] b))
        {
            if (b != null && b.Length > 0)
            {
                var base64 = b.ToBase64();

                return base64.FromBase64().Deobfuscate(Attribute.Salt);
            }
        }
        return null;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
