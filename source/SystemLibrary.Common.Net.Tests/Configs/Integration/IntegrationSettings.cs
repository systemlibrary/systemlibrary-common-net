namespace SystemLibrary.Common.Net.Tests.Configs
{
    public class IntegrationSettings : Config<IntegrationSettings>
    {
        public string FirstName { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
        public bool IsEnabled { get; set; }
    }
}
