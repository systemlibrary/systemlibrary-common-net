using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net
{
    internal class EnumStringConverterFactory : JsonConverterFactory
    {
        static Type EnumStringConverterGenericType = typeof(EnumStringConverter<>);

        public EnumStringConverterFactory(JsonNamingPolicy namingPolicy = null, bool allowIntegerValues = true)
        {
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            //TODO: Consider making this a singleton, created once
            return (JsonConverter)Activator.CreateInstance(EnumStringConverterGenericType.MakeGenericType(typeToConvert));
        }
    }
}
