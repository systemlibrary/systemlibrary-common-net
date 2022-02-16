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
    ///         }
    ///     },
    ///     ...
    /// }
    /// </code>
    /// </example>
    internal class AppSettingsConfig : Config<AppSettingsConfig>
    {
        public class Configuration
        {
            public class DumpConfiguration
            {
                public string Folder { get; set; }
                public string FileName { get; set; }

                public string GetFullLogPath()
                {
                    if (FileName.IsNot())
                    {
                        if (Folder.IsNot())
                            return "C:\\Logs\\SysLib.log";
                        else
                            return Folder + "\\SysLib.log";
                    }
                    else if (Folder.IsNot())
                        return "C:\\Logs\\" + FileName;

                    return Folder + "\\" + FileName;
                }
            }

            public DumpConfiguration Dump { get; set; }
        }

        public Configuration SystemLibraryCommonNet { get; set; }
    }
}
