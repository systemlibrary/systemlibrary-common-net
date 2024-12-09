using System;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

internal class ObfuscateJsonConverterFactory
{
    JsonObfuscateAttribute Attribute;

    public ObfuscateJsonConverterFactory(JsonObfuscateAttribute attribute)
    {
        Attribute = attribute;
    }

    public JsonConverter GetConverter(Type targetType)
    {
        if (targetType == SystemType.StringType)
            return new ObfuscateJsonStringConverter(Attribute);

        if (targetType == SystemType.IntType)
            return new ObfuscateJsonIntConverter(Attribute);

        if (targetType == SystemType.Int64Type)
        {
            return new ObfuscateJsonInt64Converter(Attribute);
        }

        throw new NotSupportedException($"Type {targetType} is not supported for JSON encryption.");
    }
}
