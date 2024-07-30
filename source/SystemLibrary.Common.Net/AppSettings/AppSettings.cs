namespace SystemLibrary.Common.Net;

internal class AppSettings : Config<AppSettings>
{
    public AppSettings()
    {
        SystemLibraryCommonNet = new PackageConfig();
    }
   
    public PackageConfig SystemLibraryCommonNet { get; set; }
}