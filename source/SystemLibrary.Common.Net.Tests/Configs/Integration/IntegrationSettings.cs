using SystemLibrary.Common.Net.Attributes;

namespace SystemLibrary.Common.Net.Tests.Configs
{
    public class IntegrationSettings : Config<IntegrationSettings>
    {
        public string FirstName { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
        public bool IsEnabled { get; set; }

        public string Password { get; set; }
        public string PasswordSecond { get; set; }

        public string PasswordDecrypt { get; set; }

        public string PasswordDecrypted { get; set; }

        [Decrypt(nameof(Password))]
        public string HelloWorld { get; set; }

        [Decrypt(nameof(PasswordSecond))]
        public string HelloWorld2 { get; set; }
    }
}
