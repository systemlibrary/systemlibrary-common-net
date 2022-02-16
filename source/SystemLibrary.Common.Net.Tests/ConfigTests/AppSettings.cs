namespace SystemLibrary.Common.Net.Tests.Configs
{
    public class AppSettingsTests : Config<AppSettingsTests>
    {
        public string FirstName { get; set; }
        public string lastname { get; set; }
        public bool isEnabled { get; set; }
    }
}
