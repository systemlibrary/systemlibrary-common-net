using System.Configuration;

namespace SystemLibrary.Common.Net.Tests.Models
{
    public class TestCollection : ConfigCollection<TestCollectionItem>
    {
        protected override object GetElementKey(ConfigurationElement element) => ((TestCollectionItem)element).Text + "";
    }
}
