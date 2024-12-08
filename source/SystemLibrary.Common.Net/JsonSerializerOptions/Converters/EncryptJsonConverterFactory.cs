using System;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

internal class EncryptJsonConverterFactory
{
    JsonEncryptAttribute Attribute;

    public EncryptJsonConverterFactory(JsonEncryptAttribute attribute)
    {
        Attribute = attribute;
    }

    public JsonConverter GetConverter(Type targetType)
    {
        if (targetType == SystemType.IntType)
            return new JsonEncryptIntConverter(Attribute);

        if (targetType == SystemType.Int64Type)
            return new JsonEncryptInt64Converter(Attribute);

        if (targetType == SystemType.StringType)
            return new JsonEncryptStringConverter(Attribute);

        throw new NotSupportedException($"Type {targetType} is not supported for JSON encryption.");
    }
}
