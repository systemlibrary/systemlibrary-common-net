using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net;

internal class JsonEncryptIntConverter : JsonEncryptBaseConverter<int>
{
    public JsonEncryptIntConverter(JsonEncryptAttribute attribute) : base(attribute)
    {
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return int.Parse(Decrypt(ref reader) ?? "0");
    }
}

internal class JsonEncryptInt64Converter : JsonEncryptBaseConverter<long>
{
    public JsonEncryptInt64Converter(JsonEncryptAttribute attribute) : base(attribute)
    {
    }

    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return long.Parse(Decrypt(ref reader) ?? "0");
    }
}

internal class JsonEncryptStringConverter : JsonEncryptBaseConverter<string>
{
    public JsonEncryptStringConverter(JsonEncryptAttribute attribute) : base(attribute)
    {
    }

    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Decrypt(ref reader);
    }
}

internal class JsonEncryptBaseConverter<T> : JsonConverter<T>
{
    JsonEncryptAttribute Attribute;

    public JsonEncryptBaseConverter(JsonEncryptAttribute attribute)
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
        if(reader.TryGetBytesFromBase64(out byte[] b))
        {
            var base64 = b.ToBase64();

            return base64.Decrypt(Attribute.Key, Attribute.IV, Attribute.AddedIV);
        }

        if (reader.TryGetInt64(out long l)) return l.ToString();
        if (reader.TryGetInt32(out int i)) return i.ToString();

        return reader.GetString();
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
