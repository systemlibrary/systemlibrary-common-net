using System;
using System.Text.Json;

namespace SystemLibrary.Common.Net;

internal class ObfuscateJsonStringConverter : BaseObfuscateJsonConverter<string>
{
    public ObfuscateJsonStringConverter(JsonObfuscateAttribute attribute) : base(attribute)
    {
    }

    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return Deobfuscate(ref reader);
        }
        catch
        {
            return reader.GetString();
        }
    }
}
