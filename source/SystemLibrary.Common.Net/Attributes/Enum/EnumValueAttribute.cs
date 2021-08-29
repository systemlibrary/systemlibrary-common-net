using System;

namespace SystemLibrary.Common.Net.Attributes
{
    /// <summary>
    /// Enum value attribute on any enum key
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// enum Color 
    /// {
    ///     [EnumValue("#000")]
    ///     Black,
    ///     White
    /// }
    /// 
    /// var color = Color.Black;
    /// var value = color.ToValue();
    /// //value == "#000"
    /// 
    /// var color = Color.White;
    /// var value = color.ToValue();
    /// //value == "White"
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumValueAttribute : Attribute
    {
        public string Value;

        /// <param name="value">Set additional data metadata for the Enum key</param>
        public EnumValueAttribute(object value)
        {
            this.Value = value + "";
        }
    }
}