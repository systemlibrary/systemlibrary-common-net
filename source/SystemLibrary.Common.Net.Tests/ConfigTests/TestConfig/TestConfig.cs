using System.Configuration;

namespace SystemLibrary.Common.Net.Tests.Models
{
    public class TestConfig : Config<TestConfig>
    {
        [ConfigurationProperty(nameof(Name), IsRequired = true)]
        public string Name => Get(nameof(Name));

        [ConfigurationProperty(nameof(List))]
        public TestCollection List => this[nameof(List)] as TestCollection;
    }
}
