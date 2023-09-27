using System;
using System.Linq;
using System.Text.Json;

namespace SystemLibrary.Common.Net;

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
///             "folder": "C:\\logs\\", // or %HomeDrive%\\logs\\ which works on MacOs too. Note: can maximum contain one env variable
///             "fileName": "output.log",
///             "debug": true // or false to avoid dumping internal warnings and errors
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
            public bool Dump { get; set; }
            public DumpConfiguration()
            {
                Folder = "%HomeDrive%\\Logs\\";
                FileName = "SysLib.log";
                Dump = false;
            }

            public string GetFullLogPath()
            {
                var firstIndex = Folder.IndexOf('%');
                if (firstIndex > -1)
                {
                    var lastIndex = Folder.LastIndexOf('%');

                    if (firstIndex == lastIndex) throw new Exception("Log folder cannot contain only one %, specify for instance: %HomeDrive%");

                    var varName = Folder.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                    var value = Environment.GetEnvironmentVariable(varName);

                    if (!Folder.EndsWith("%"))
                        Folder = value + "\\" + Folder.Substring(lastIndex + 1);
                    else
                        Folder = value + "\\";
                }

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
