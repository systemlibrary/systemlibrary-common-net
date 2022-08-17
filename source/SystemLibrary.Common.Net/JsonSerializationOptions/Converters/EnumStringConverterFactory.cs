using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net
{
    internal class EnumStringConverterFactory : JsonConverterFactory
    {
        //readonly JsonStringEnumConverter stringEnumConverter;

        public EnumStringConverterFactory(JsonNamingPolicy namingPolicy = null, bool allowIntegerValues = true)
        {
            //stringEnumConverter = new(namingPolicy, allowIntegerValues);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            //TODO: Consider passing "options" to the generic factory...
            return (JsonConverter)Activator.CreateInstance(typeof(EnumStringConverter<>).MakeGenericType(typeToConvert));
        }
    }
}
