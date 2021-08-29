using System.Configuration;

namespace SystemLibrary.Common.Net.Tests.Models
{
    public class TestCollectionItem : ConfigCollectionItem
    {
        [ConfigurationProperty(nameof(Text), IsRequired = true)]
        public string Text => Get(nameof(Text));

        [ConfigurationProperty(nameof(Number), IsRequired = true)]
        public int Number => int.Parse(Get(nameof(Number)));
    }
}
