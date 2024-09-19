using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using static SystemLibrary.Common.Net.PackageConfig;

namespace SystemLibrary.Common.Net;

static internal class _JsonSerializerOptions
{
    static AppSettings Config => AppSettings.Current;

    static IntJsonConverter IntJsonConverter = new IntJsonConverter();
    static TypeConverter TypeConverter = new TypeConverter();
    static LongJsonConverter LongJsonConverter = new LongJsonConverter();

    static void AddConverters(JsonSerializerOptions options)
    {
        if (options.Converters?.Count > 0) return;

        // NOTE: Optimize by creating these converters just once during app run time, they can be singleton IIRC
        options.Converters.Add(new StringJsonConverter());
        options.Converters.Add(IntJsonConverter);
        options.Converters.Add(new EnumStringConverterFactory());
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
        options.Converters.Add(new DateTimeOffsetJsonConverter("yyyy-MM-dd"));
        options.Converters.Add(LongJsonConverter);
        options.Converters.Add(TypeConverter);
    }

    static JsonConfig _JsonConfiguration;
    static JsonConfig JsonConfiguration
    {
        get
        {
            _JsonConfiguration ??= Config.SystemLibraryCommonNet.Json;
            return _JsonConfiguration;
        }
    }

    static JavaScriptEncoder _JavaScriptEncoder;
    static JavaScriptEncoder JavaScriptEncoder
    {
        get
        {
            _JavaScriptEncoder ??= JavaScriptEncoder.Create(
                            UnicodeRanges.BasicLatin,
                            UnicodeRanges.LatinExtendedA,
                            UnicodeRanges.LatinExtendedB,
                            UnicodeRanges.LatinExtendedAdditional,
                            UnicodeRanges.LatinExtendedC,
                            UnicodeRanges.Latin1Supplement,
                            UnicodeRanges.CurrencySymbols,
                            UnicodeRanges.Cyrillic,
                            UnicodeRanges.GreekandCoptic);
            return _JavaScriptEncoder;
        }
    }

    static JsonSerializerOptions DefaultJsonSerializerOptions
    {
        get
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder,
                DefaultIgnoreCondition = JsonConfiguration.JsonIgnoreCondition,
                MaxDepth = JsonConfiguration.MaxDepth,
                AllowTrailingCommas = JsonConfiguration.AllowTrailingCommas,
                PropertyNameCaseInsensitive = JsonConfiguration.PropertyNameCaseInsensitive,
                WriteIndented = JsonConfiguration.WriteIndented,
                PropertyNamingPolicy = null,
                IncludeFields = true,
                ReadCommentHandling = JsonConfiguration.ReadCommentHandling,
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
                    options.Converters.Insert(0, converter);
            }
        }
        else
        {
            AddConverters(options);

            if (options.MaxDepth <= 0)
                options.MaxDepth = DefaultJsonSerializerOptions.MaxDepth;
        }

        return options;
    }
}
