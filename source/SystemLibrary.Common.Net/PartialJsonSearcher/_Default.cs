using System.Text.Json;

namespace SystemLibrary.Common.Net
{
    partial class PartialJsonSearcher
    {
        static AppSettingsConfig Config => AppSettingsConfig.Current;

        static JsonSerializerOptions _DefaultJsonSerializerOptions;
        static JsonSerializerOptions DefaultJsonSerializerOptions => 
            _DefaultJsonSerializerOptions != null ? _DefaultJsonSerializerOptions 
            : (_DefaultJsonSerializerOptions = new JsonSerializerOptions {
                MaxDepth = Config.SystemLibraryCommonNet.Json.MaxDepth,
                AllowTrailingCommas = Config.SystemLibraryCommonNet.Json.AllowTrailingCommas,
                ReadCommentHandling = JsonCommentHandling.Skip,
                PropertyNameCaseInsensitive = Config.SystemLibraryCommonNet.Json.PropertyNameCaseInsensitive,
                WriteIndented = Config.SystemLibraryCommonNet.Json.WriteIndented,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
       
        internal static JsonSerializerOptions Default(JsonSerializerOptions options)
        {
            if (options != null)
            {
                if (options.MaxDepth < 4)
                    options.MaxDepth = 4;

                if (options.MaxDepth > 256)
                    options.MaxDepth = 256;

                return options;
            }

            return DefaultJsonSerializerOptions;
        }
    }
}
