using System.Text.Json;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Override default configurations in 'SystemLibrary.Common.Net' by adding 'systemLibraryCommonNet' object to 'appSettings.json'
    /// </summary>
    /// <example>
    /// 'appSettings.json'
    /// <code class="language-csharp hljs">
    /// {
    ///     ...,
    ///     "systemLibraryCommonNet": {
    ///         "dump": {
    ///             "folder": "C:\\logs\\",
    ///             "fileName": "output.log"
    ///         },
    ///         "json": {
    ///             "writeIndented": false,
    ///             "maxDepth": 16,
    ///             "allowTrailingCommas": true,
    ///             "propertyNameCaseInsensitive": true
    ///         }
    ///     },
    ///     ...
    /// }
    /// </code>
    /// </example>
    internal class AppSettings : Config<AppSettings>
    {
        public AppSettings()
        {
            SystemLibraryCommonNet = new Configuration();
        }

        public class Configuration
        {
            public Configuration()
            {
                Dump = new DumpConfiguration();
                Json = new JsonConfiguration();
            }

            public class JsonConfiguration
            {
                public int MaxDepth { get; set; } = 32;
                public bool AllowTrailingCommas { get; set; } = true;
                public bool PropertyNameCaseInsensitive { get; set; } = true;
                public bool WriteIndented { get; set; } = false;
                public JsonCommentHandling ReadCommentHandling { get; set; } = JsonCommentHandling.Skip;
            }

            public class DumpConfiguration
            {
                public string Folder { get; set; }
                public string FileName { get; set; }

                public DumpConfiguration()
                {
                    FileName = "SysLib.log";
                    Folder = "C:\\Logs";
                }

                public string GetFullLogPath()
                {
                    if (Folder.EndsWith("\\"))
                        return Folder + FileName;

                    return Folder + "\\" + FileName;
                }
            }

            public DumpConfiguration Dump { get; set; }
            public JsonConfiguration Json { get; set; }
        }

        public Configuration SystemLibraryCommonNet { get; set; }
    }
}
