using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net
{
    static internal class GetJsonSerializerOptions
    {
        static AppSettings Config => AppSettings.Current;

        static JsonSerializerOptions _DefaultJsonSerializerOptions;
        static JsonSerializerOptions DefaultJsonSerializerOptions
        {
            get
            {
                if (_DefaultJsonSerializerOptions == null)
                {
                    _DefaultJsonSerializerOptions = new JsonSerializerOptions
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

                    _DefaultJsonSerializerOptions.Converters.Add(new EnumStringConverterFactory());
                    _DefaultJsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    _DefaultJsonSerializerOptions.Converters.Add(new IntJsonConverter());
                    _DefaultJsonSerializerOptions.Converters.Add(new StringJsonConverter());
                    _DefaultJsonSerializerOptions.Converters.Add(new LongJsonConverter());
                    _DefaultJsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd"));
                    _DefaultJsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter("yyyy-MM-dd"));
                }
                return _DefaultJsonSerializerOptions;
            }
        }

        internal static JsonSerializerOptions Default(JsonSerializerOptions options)
        {
            if (options == null) return DefaultJsonSerializerOptions;

            if (options.MaxDepth < 2)
                options.MaxDepth = 2;

            if (options.MaxDepth > 512)
                options.MaxDepth = 512;

            return options;
        }
    }
}
