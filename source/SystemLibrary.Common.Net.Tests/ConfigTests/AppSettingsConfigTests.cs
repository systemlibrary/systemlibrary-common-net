namespace SystemLibrary.Common.Net.Tests.Configs;

public class AppSettingsConfigTests : Config<AppSettingsConfigTests>
{
    public string DummyUser { get; set; }
    public Parent Parent { get; set; }
    public AppSettingsConfigTests()
    {
        Parent = new Parent();
    }
}
