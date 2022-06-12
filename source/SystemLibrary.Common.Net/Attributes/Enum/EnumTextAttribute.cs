using System;

namespace SystemLibrary.Common.Net.Attributes
{
    /// <summary>
    /// Add additional string data to an Enum Key
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// enum Color 
    /// {
    ///     [EnumText("Black Colored Text")]
    ///     Black,
    ///     White
    /// }
    /// 
    /// var color = Color.Black;
    /// 
    /// var value = color.ToText();
    /// 
    /// var value2 = Color.White.ToText();
    /// 
    /// //'value' is now "Black Colored Text"
    /// //'value2' is now "White", 
    /// //If EnumText attribute do not exist, it falls back to .ToString() of the field
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