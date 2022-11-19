using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net
{
    static internal class GetJsonSerializerOptions
    {
        static AppSettings Config => AppSettings.Current;

        static JsonSerializerOptions _DefaultJsonSerializerOptions;

        static JsonSerializerOptions NewJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                MaxDepth = Config.SystemLibraryCommonNet.Json.MaxDepth,
                AllowTrailingCommas = Config.SystemLibraryCommonNet.Json.AllowTrailingCommas,
                PropertyNameCaseInsensitive = Config.SystemLibraryCommonNet.Json.PropertyNameCaseInsensitive,
                WriteIndented = Config.SystemLibraryCommonNet.Json.WriteIndented,
                PropertyNamingPolicy = null,
                IncludeFields = true,
                ReadCommentHandling = Config.SystemLibraryCommonNet.Json.ReadCommentHandling,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            options.Converters.Add(new EnumStringConverterFactory());
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new IntJsonConverter());
            options.Converters.Add(new StringJsonConverter());
            options.Converters.Add(new LongJsonConverter());
            options.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd"));
            options.Converters.Add(new DateTimeOffsetJsonConverter("yyyy-MM-dd"));

            return options;
        }

        static JsonSerializerOptions DefaultJsonSerializerOptions
        {
            get
            {
                if (_DefaultJsonSerializerOptions == null)
                {
                    _DefaultJsonSerializerOptions = NewJsonSerializerOptions();
                }
                return _DefaultJsonSerializerOptions;
            }
        }

        internal static JsonSerializerOptions Default(JsonSerializerOptions options, params JsonConverter[] converters)
        {
            if (options == null && converters == null) return DefaultJsonSerializerOptions;

            if (options == null) options = NewJsonSerializerOptions();
            else
            {
                if (options.MaxDepth < 2)
                    options.MaxDepth = DefaultJsonSerializerOptions.MaxDepth;

                if (options.MaxDepth > 1024)
                    options.MaxDepth = 1024;

                if (options.PropertyNamingPolicy == null)
                    options.PropertyNamingPolicy = DefaultJsonSerializerOptions.PropertyNamingPolicy;
            }

            if(options.Converters.Count == 0)
            {
                options.Converters.Add(new EnumStringConverterFactory());
                options.Converters.Add(new JsonStringEnumConverter());
                options.Converters.Add(new IntJsonConverter());
                options.Converters.Add(new StringJsonConverter());
                options.Converters.Add(new LongJsonConverter());
                options.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd"));
                options.Converters.Add(new DateTimeOffsetJsonConverter("yyyy-MM-dd"));

                if(converters != null)
                {
                    foreach (var converter in converters)
                        options.Converters.Add(converter);
                }
            }
            
            return options;
        }
    }
}
