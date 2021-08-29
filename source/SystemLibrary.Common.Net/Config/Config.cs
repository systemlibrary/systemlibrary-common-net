using System;
using System.Configuration;
using System.Net;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class for creating a custom configuration node in your app or web.config
    /// </summary>
    ///<example>
    /// A TestConfig class example. 
    ///
    /// See documentation on ConfigCollection for an example of the "TestCollection" variable in the example below.
    /// <code class="language-csharp hljs">
    /// public class TestConfig : Config&lt;TestConfig&gt; 
    /// {
    ///    [ConfigurationProperty(nameof(Name), IsRequired = true)]
    ///    public string Name =&gt; Get(nameof(Name));
    ///     
    ///    [ConfigurationProperty(nameof(List))]
    ///    public TestCollection List =&gt; this[nameof(List)] as TestCollection;
    /// }
    /// </code>
    /// 
    /// Add to your config file:
    /// <code class="language-xml hljs">
    /// &lt;section name="testConfig" type="SystemLibrary.Common.Net.TestConfig, SystemLibrary.Common.Net"&gt;
    /// 
    /// &lt;testConfig name="hello"&gt;
    ///     &lt;List&gt;
    ///     &lt;test Text="A" Number="1" /&gt;
    ///     &lt;/List&gt;
    /// &lt;/testConfig&gt;
    /// </code>
    /// 
    /// <code>
    /// var testConfig = TestConfig.Current;
    /// var name = testConfig.Name;
    /// //name == hello
    /// </code>
    /// </example>
    /// <typeparam name="T">T is the class inheriting Config&lt;&gt;, also referenced as 'self'. Note that T cannot be a nested class</typeparam>
    public abstract class Config<T> : ConfigurationSection where T : ConfigurationSection
    {
        static T _Config;

        /// <summary>
        /// Reads the configuration section and returns the configuration as a singleton
        /// </summary>
        public static T Current
        {
            get
            {
                if (_Config == default)
                {
                    _Config = GetSectionBySectionName();

                    if (_Config == default)
                        _Config = Activator.CreateInstance<T>();
                }
                return _Config;
            }
        }

        static T GetSectionBySectionName()
        {
            //NOTE: Root node in your custom .config file must match any of these section name formats
            var name = typeof(T).Name;
            var nameLowerFirstChar = char.ToLower(name[0]) + name.Substring(1);

            var nameWithoutSection = name.Replace("Section", "");
            var nameLowerfirstCharWithoutSection = char.ToLower(nameWithoutSection[0]) + nameWithoutSection.Substring(1);

            return (ConfigurationManager.GetSection(name)
                ?? ConfigurationManager.GetSection(nameLowerFirstChar)
                ?? ConfigurationManager.GetSection(nameWithoutSection)
                ?? ConfigurationManager.GetSection(nameLowerfirstCharWithoutSection)
                ?? ConfigurationManager.GetSection(name.ToLower())) as T;
        }

        /// <summary>
        /// Returns the value of the 'key' from the XML section element
        /// 
        /// Note: Before the value is returned the string is also HtmlDecoded
        /// </summary>
        /// <example>
        /// Use this Get method to retrieve the value from the XML
        /// <code class="language-csharp hljs">
        /// [ConfigurationProperty(nameof(Name), IsRequired = true)]
        /// public string Name =&gt; Get(nameof(Name));
        /// </code>
        /// 
        /// Or you can also convert the XML value directly
        /// <code class="language-csharp hljs">
        /// [ConfigurationProperty(nameof(Age), IsRequired = true)]
        /// public string Age =&gt; int.parse(Get(nameof(Age)));
        /// </code>
        /// </example>
        protected string Get(string key) =>  WebUtility.HtmlDecode(this[key]?.ToString());
    }
}
