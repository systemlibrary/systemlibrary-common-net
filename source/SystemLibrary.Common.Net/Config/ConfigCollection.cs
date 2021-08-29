using System;
using System.Configuration;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Create a ConfigCollection of T that is used within your custom Config&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">T must be a class inheriting ConfigurationElement</typeparam>
    /// <example>
    /// <code class="language-csharp hljs">
    /// public class TestCollection : ConfigCollection&lt;TestCollectionItem&gt;
    /// {
    ///    protected override object GetElementKey(ConfigurationElement element) => ((TestCollectionItem)element).Text + "";
    /// }
    /// </code>
    /// 
    /// See documentation on ConfigCollectionItem for an example of the "TestCollectionItem" above 
    ///</example>
    public abstract class ConfigCollection<T> : ConfigurationElementCollection where T : ConfigurationElement
    {
        protected override string ElementName => GetType().Name;

        /// <summary>
        /// Used internally
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        /// <summary>
        /// Used internally
        /// </summary>
        public T this[object key]
        {
            get
            {
                return (T)base.BaseGet(key);
            }
        }

        /// <summary>
        /// Used internally
        /// </summary>
        protected override bool IsElementName(string name)
        {
            var nameLowered = name.ToLower();

            var elementNameLowered = this.ElementName.ToLower()
                .Replace("elementcollection", "")
                .Replace("collection", "");

            return elementNameLowered == nameLowered;
        }

        /// <summary>
        /// Used internally
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
