using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

static internal class GetJsonSerializerOptions
{
    static AppSettings Config => AppSettings.Current;

    static JsonSerializerOptions _DefaultJsonSerializerOptions;

    static void AddConverters(JsonSerializerOptions options)
    {
        if (options.Converters?.Count > 0) return;

        options.Converters.Add(new EnumStringConverterFactory());
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new IntJsonConverter());
        options.Converters.Add(new StringJsonConverter());
        options.Converters.Add(new LongJsonConverter());
        options.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd"));
        options.Converters.Add(new DateTimeOffsetJsonConverter("yyyy-MM-dd"));
        options.Converters.Add(new TypeConverter());
    }

    static JsonSerializerOptions DefaultJsonSerializerOptions
    {
        get
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
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };

            AddConverters(options);

            return options;
        }
    }

    internal static JsonSerializerOptions Default(JsonSerializerOptions options, params JsonConverter[] converters)
    {
        if (options == null && converters == null) return DefaultJsonSerializerOptions;

        if (options == null)
        {
            options = DefaultJsonSerializerOptions;

            if (converters != null)
            {
                foreach (var converter in converters)
                    options.Converters.Add(converter);
            }
        }
        else
        {
            AddConverters(options);

            if (options.MaxDepth < 2)
                options.MaxDepth = DefaultJsonSerializerOptions.MaxDepth;
        }
        
        return options;
    }
}
