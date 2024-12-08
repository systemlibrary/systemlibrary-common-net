using System;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JsonEncryptAttribute : JsonConverterAttribute
{
    internal string Key;
    internal string IV;
    internal bool AddedIV;

    public JsonEncryptAttribute(string key = null, string IV = null, bool addedIV = true)
    {
        this.Key = key;
        this.IV = IV;
        this.AddedIV = addedIV;
    }

    public override JsonConverter CreateConverter(Type typeToConvert)
    {
        return new EncryptJsonConverterFactory(this).GetConverter(typeToConvert);
    }
}
