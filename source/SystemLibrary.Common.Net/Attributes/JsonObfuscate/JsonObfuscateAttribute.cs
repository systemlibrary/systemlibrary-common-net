using System;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JsonObfuscateAttribute : JsonConverterAttribute
{
    internal int Salt;

    public JsonObfuscateAttribute(int salt = 77)
    {
        Salt = salt;
    }

    public override JsonConverter CreateConverter(Type typeToConvert)
    {
        return new ObfuscateJsonConverter(this);
    }
}
