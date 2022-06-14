using System.Text.Json;

namespace SystemLibrary.Common.Net
{
    partial class PartialJsonSearcher
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
                        MaxDepth = Config.SystemLibraryCommonNet.Json.MaxDepth,
                        AllowTrailingCommas = Config.SystemLibraryCommonNet.Json.AllowTrailingCommas,
                        PropertyNameCaseInsensitive = Config.SystemLibraryCommonNet.Json.PropertyNameCaseInsensitive,
                        WriteIndented = Config.SystemLibraryCommonNet.Json.WriteIndented,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        ReadCommentHandling = Config.SystemLibraryCommonNet.Json.ReadCommentHandling
                    };
                }
                return _DefaultJsonSerializerOptions;
            }
        }
       
        internal static JsonSerializerOptions Default(JsonSerializerOptions options)
        {
            if (options != null)
            {
                if (options.MaxDepth < 2)
                    options.MaxDepth = 2;

                if (options.MaxDepth > 256)
                    options.MaxDepth = 256;

                return options;
            }

            return DefaultJsonSerializerOptions;
        }
    }
}
