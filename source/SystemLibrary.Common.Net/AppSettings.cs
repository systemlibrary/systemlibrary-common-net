using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            public bool AllowTrailingCommas { get; set; } = true;
            public int MaxDepth { get; set; } = 32;
            public bool PropertyNameCaseInsensitive { get; set; } = true;
            public JsonCommentHandling ReadCommentHandling { get; set; } = JsonCommentHandling.Skip;
            public JsonIgnoreCondition JsonIgnoreCondition { get; set; } = JsonIgnoreCondition.WhenWritingNull;
            public bool WriteIndented { get; set; } = false;
        }

        public class DumpConfiguration
        {
            public string Folder { get; set; }
            public string FileName { get; set; }
            public bool Debug { get; set; }
            public DumpConfiguration()
            {
                Folder = "%HomeDrive%\\Logs\\";
                FileName = "DumpWrite.log";
                Debug = false;
            }

            public string GetFullLogPath()
            {
                var firstIndex = Folder.IndexOf('%');

                if (firstIndex > -1)
                {
                    var lastIndex = Folder.LastIndexOf('%');

                    if (firstIndex == lastIndex) throw new Exception("Log folder cannot contain only one %, specify for instance: %HomeDrive%");

                    var varName = Folder.Substring(firstIndex + 1, lastIndex - firstIndex - 1);

                    var value = "";

                    try
                    {
                        value = Environment.GetEnvironmentVariable(varName);
                    }
                    catch
                    {
                    }
                    if (value.IsNot())
                    {
                        try
                        {
                            value = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);
                        }
                        catch
                        {
                        }
                    }

                    if (value.IsNot() && varName.ToLower() == "homedrive")
                    {
                        value = Environment.GetLogicalDrives()?[0];
                    }

                    Folder = Folder.Replace("%" + varName + "%", value);
                }

                if (FileName.StartsWith("\\"))
                    FileName = FileName.Substring(1);

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
