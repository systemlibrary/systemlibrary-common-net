using System.Configuration;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class for config collection items
    /// 
    /// Ex: public class TestCollectionItem : ConfigCollectionItem
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// public class TestCollectionItem : ConfigCollectionItem
    /// {
    ///     [ConfigurationProperty(nameof(Text), IsRequired = true)]
    ///     public string Text => Get(nameof(Text));
    ///     
    ///     [ConfigurationProperty(nameof(Number), IsRequired = true)]
    ///     public int Number => int.Parse(Get(nameof(Number)));
    /// }
    /// </code>
    ///</example>
    public abstract class ConfigCollectionItem : ConfigurationElement
    {
        protected string Get(string key) => this[key]?.ToString();
    }
}
