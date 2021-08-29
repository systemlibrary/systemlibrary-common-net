using System;

namespace SystemLibrary.Common.Net.Attributes
{
    /// <summary>
    /// Enum text attribute on any enum key
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// enum Color 
    /// {
    ///     [EnumText("Black Colored Text")]
    ///     Black
    /// }
    /// 
    /// var color = Color.Black;
    /// var value = color.ToText();
    /// //value == "Black Colored Text"
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumTextAttribute : Attribute
    {
        public string Text { get; set; }

        /// <param name="text">Set additional text metadata for the Enum key</param>
        public EnumTextAttribute(string text = null)
        {
            Text = text;
        }
    }
}