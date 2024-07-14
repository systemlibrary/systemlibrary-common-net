using SystemLibrary.Common.Net.Attributes;

namespace SystemLibrary.Common.Net.Tests.Configs;

public class AppSettingsConfigTests : Config<AppSettingsConfigTests>
{
    public string DummyUser { get; set; }
    public Parent Parent { get; set; }

    public string Password { get; set; }
    public string PasswordSecond { get; set; }

    public string PasswordDecrypt { get; set; }

    public string PasswordDecrypted { get; set; }

    [Decrypt(nameof(Password))]
    public string HelloWorld { get; set; }

    [Decrypt(nameof(PasswordSecond))]
    public string HelloWorld2 { get; set; }

    public AppSettingsConfigTests()
    {
        Parent = new Parent();
    }
}
