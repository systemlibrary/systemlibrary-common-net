using System;

namespace SystemLibrary.Common.Net.Attributes;

/// <summary>
/// Add additional string data to an Enum Key
/// </summary>
/// <example>
/// <code class="language-csharp hljs">
/// enum Color 
/// {
///     [EnumValue("#000")]
///     [EnumText("Hello Black")]
///     Black,
///     
///     [EnumText("Hello White")]
///     White,
///     
///     Pink
/// }
/// 
/// var black = Color.Black;
/// var value = black.ToValue();
/// //'value' is now "#000"
/// 
/// var value = Color.White.ToValue();
/// //'value' is now "White"
/// 
/// var value = Color.Pink.ToValue();
/// //'value' is now 'Pink'
/// 
/// //If EnumValue attribute do not exist, it falls back to .ToString() on the Key
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumValueAttribute : Attribute
{
    public object Value;

    public EnumValueAttribute(object value)
    {
        this.Value = value;
    }
}