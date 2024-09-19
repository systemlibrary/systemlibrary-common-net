using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net;

internal class PackageConfig
{
    public PackageConfig()
    {
        Dump = new DumpConfig();
        Json = new JsonConfig();
        Debug = false;
    }

    public class JsonConfig
    {
        public bool AllowTrailingCommas { get; set; } = true;
        public int MaxDepth { get; set; } = 32;
        public bool PropertyNameCaseInsensitive { get; set; } = true;
        public JsonCommentHandling ReadCommentHandling { get; set; } = JsonCommentHandling.Skip;
        public JsonIgnoreCondition JsonIgnoreCondition { get; set; } = JsonIgnoreCondition.WhenWritingNull;
        public bool WriteIndented { get; set; } = false;
    }

    public class DumpConfig
    {
        public string Folder { get; set; }
        public string FileName { get; set; }
        public DumpConfig()
        {
            Folder = "%HomeDrive%/Logs/";
            FileName = "DumpWrite.log";
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

            if (FileName.StartsWith("/"))
                FileName = FileName.Substring(1);

            if (Folder.EndsWith("\\"))
                return Folder + FileName;

            if (Folder.EndsWith("/"))
                return Folder + FileName;

            return Folder + "/" + FileName;
        }
    }

    public bool Debug { get; set; }
    public DumpConfig Dump { get; set; }
    public JsonConfig Json { get; set; }
}