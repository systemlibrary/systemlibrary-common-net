using System;
using System.Linq;
using System.Reflection;

using SystemLibrary.Common.Net;
using SystemLibrary.Common.Net.Attributes;
using SystemLibrary.Common.Net.Extensions;

/// <summary>
/// Extension methods for strings
/// 
/// They are living in the Global namespace, so they are always available anyhwere in your code
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns the first of the parameters sent to this method that is not null, blank or only a single space.
    /// If current, and all 'optional' fallback values does not meet the condition, "" is returned, never null
    /// </summary>
    /// <returns>First non-null, non-empty and non-space string value, or empty string. Returns never null.</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text1 = null;
    /// var text2 = "";
    /// var text3 = " ";
    /// var text4 = "hello";
    /// 
    /// var result = text1.AsFallback(text2, text3, text4);
    /// //result is "hello" as the others are blank/empty
    /// </code>
    /// </example>
    public static string AsFallback(this string current, params string[] fallbacks)
    {
        if (current.Is()) return current;

        if (fallbacks == null || fallbacks.Length == 0) return "";

        foreach (var fallback in fallbacks)
            if (!fallback.IsNot())
                return fallback;

        return "";
    }

    /// <summary>
    /// Convert a string value to the Enum of your choice
    /// </summary>
    /// <typeparam name="T">T must be an Enum</typeparam>
    /// <param name="value">Value must match the Key or the 'EnumValueAttribute' of a Key in the Enum, else this returns default of the Enum T</param>
    /// <returns>Returns first matching Key or default of the Enum</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// enum EnumColor
    /// {
    ///     [EnumText("White")]
    ///     [EnumValue("BlackAndWhite")]
    ///     Black,
    ///     
    ///     Pink
    /// }
    ///
    /// var value = "black".ToEnum&lt;EnumColor&gt;();
    /// //value is EnumColor.Black, case insensitive conversion
    /// 
    /// var value = "white".ToEnum&lt;EnumColor&gt;();
    /// //value is EnumColor.Black, case insensitive conversion and enum text match
    /// 
    /// var value = "blackAndWhite".ToEnum&lt;EnumColor&gt;();
    /// //value is EnumColor.Black, case insensitive conversion and enum value match
    /// 
    /// var value = "brown".ToEnum&lt;EnumColor&gt;();
    /// //value is EnumColor.Black, no match, returns first/default
    /// </code>
    /// </example>
    public static T ToEnum<T>(this string value) where T : struct, IComparable, IFormattable, IConvertible
    {
        T result;
        var type = typeof(T);

        if (type.IsEnum)
        {
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

            if (members?.Length > 0)
            {
                value = value?.ToLower();

                foreach(var enumKey in members)
                {
                    if(enumKey.GetCustomAttribute(SystemType.EnumValueAttributeType) is EnumValueAttribute enumValueAttribute)
                    {
                        if(enumValueAttribute != null && enumValueAttribute.Value?.ToLower() == value)
                            if (Enum.TryParse(enumKey.Name, out result))
                                return result;
                    }
                    
                    if (enumKey.GetCustomAttribute(SystemType.EnumTextAttributeType) is EnumTextAttribute enumTextAttribute)
                    {
                        if (enumTextAttribute != null && enumTextAttribute.Text?.ToLower() == value)
                            if (Enum.TryParse(enumKey.Name, out result))
                                return result;
                    }
                }
            }
        }

        if (value.IsNot()) return default;

        if (Enum.TryParse(value, true, out result))
            return result;

        return result;
    }

    /// <summary>
    /// Check if a text strats with any of the values, case sensitive
    /// </summary>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = "hello world";
    /// 
    /// var result = text.StartsWithAny("", "abcdef", "hel");
    /// //result is true, due to the text begins with 'hel'
    /// </code>
    /// </example>
    public static bool StartsWithAny(this string text, params string[] values)
    {
        if (text.IsNot()) return false;

        if (values.IsNot()) return false;

        for (int i = 0; i < values.Length; i++)
            if (text.StartsWith(values[i]))
                return true;

        return false;
    }

    /// <summary>
    /// Check if text ends with any of the value, case sensitive
    /// </summary>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = "hello world";
    /// var result = text.EndsWithAny("", "abdef", "rld");
    /// //result is true, because the last part of text ends with rld
    /// </code>
    /// </example>
    public static bool EndsWithAny(this string text, params string[] values)
    {
        if (text.IsNot()) return false;
        if (values.IsNot()) return false;

        if (values.Any(value => text.EndsWith(value)))
            return true;

        return false;
    }

    /// <summary>
    /// Check if text is any of the values, case sensitive
    /// </summary>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = "hello world";
    /// var result = text.IsAny("hello", "world", "hello WORLD", "hello world");
    /// //result is true, as the last 'hello world' matches exactly
    /// </code>
    /// </example>
    public static bool IsAny(this string text, params string[] values)
    {
        if (text.IsNot()) return false;

        if (values.IsNot()) return false;

        for (int i = 0; i < values.Length; i++)
            if (text == values[i])
                return true;

        return false;
    }

    /// <summary>
    /// Check if text is null, "" or " ", returns true if so, otherwise false
    /// 
    /// Note: it does not check if it contains multiple spaces so it is not exactly as string.IsNullOrWhiteSpace()
    /// </summary>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// //Old way: string.IsNullOrWhiteSpace(text);
    /// 
    /// var text = " ";
    /// var result = text.IsNot();
    /// //result is true because a single space counts as "no text" in this function
    /// 
    /// var text = "  "; //2 spaces
    /// var result = text.IsNot();
    /// //result is false because two spaces counts as text in this function
    /// </code>
    /// </example>
    public static bool IsNot(this string text)
    {
        return text == null || text == "" || text == " ";
    }

    /// <summary>
    /// Check if text is not null, not "", and not a " ", returns true if so, otherwise false
    /// </summary>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// //Old way: string.IsNullOrWhiteSpace(text);
    /// 
    /// var text = "hello world";
    /// var result = text.Is();
    /// //result is true because text is set to something, and not just "" or " "
    /// </code>
    /// </example>
    public static bool Is(this string text)
    {
        return !text.IsNot();
    }

    /// <summary>
    /// Returns a new string where all non-digit and non-letters has been removed
    /// </summary>
    /// <returns>String with digits and letters only or "", never null</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var email = "support@system.library.com";
    /// var text = email.ToLetterAndDigits();
    /// 
    /// //text is "supportsystemlibrarycom"
    /// </code>
    /// </example>
    public static string ToLetterAndDigits(this string text)
    {
        if (text.IsNot()) return "";

        return new string(text.Where(x => char.IsLetterOrDigit(x)).ToArray());
    }

    /// <summary>
    /// Check if text contains any of the values, case sensitive
    /// </summary>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = "hello";
    /// 
    /// var result = text.ContainsAny("ll");
    /// //result is true, because ll is part of the text
    /// </code>
    /// </example>
    public static bool ContainsAny(this string text, params string[] values)
    {
        if (text.IsNot()) return false;
        if (values.IsNot()) return false;

        if (values.Any(value => text.Contains(value)))
            return true;

        return false;
    }

    /// <summary>
    /// Checks if text contains any of the values, if text ends with one of the value, that values is deleted from the text and then remaning text is returned
    /// 
    /// This is not recursive, so after removal of 1 value, it will return
    /// </summary>
    /// <returns>Returns the input string as is or without one of the values</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var result = RemoveIfEndsWith("abcd", " ", "!", "c", "d");
    /// 
    /// //result is abc
    /// </code>
    /// </example>
    public static string RemoveIfEndsWith(this string text, params string[] values)
    {
        if (text.IsNot()) return text;
        if (values.IsNot()) return text;

        int start = 0;
        int valueLength;
        bool found = false;
        for(int i = 0; i < values.Length; i++)
        {
            valueLength = values[i].Length;
            start = text.Length - valueLength; 

            for (int j = 0; j < valueLength; j++)
            {
                if (text[start + j] != values[i][j])
                    break;
                
                if (j == valueLength - 1)
                    found = true;
            }

            if (found)
                break;
        }

        if (found)
            return text.Substring(0, start);

        return text;
    }

    /// <summary>
    /// Check if text contains any of the characters, case insensitive
    /// </summary>
    /// <param name="characters">Each character in this string will be checked</param>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = "hello world";
    /// 
    /// var result = text.EndsWithAnyCharacter("abcdef");
    /// //result is true, because it ends with 'd'
    /// </code>
    /// </example>
    public static bool EndsWithAnyCharacter(this string text, string characters)
    {
        if (text.IsNot()) return true;

        if (characters.IsNot()) return true;

        foreach (var character in characters)
        {
            if (text.EndsWith(character.ToString(), StringComparison.InvariantCultureIgnoreCase))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the string or a substring of the string, based on max allowed string length
    /// </summary>
    /// <returns>Returns the string as it is if text is blank/null or shorter than maxLength, else returns a substring with a length of 'maxLength'. If maxLength is negative it returns ""</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = "hello world";
    /// var result = text.MaxLength(1);
    /// //result is "h" as it only can be 1 character long
    /// </code>
    /// </example> 
    public static string MaxLength(this string text, int maxLength)
    {
        if (text.IsNot()) return text;

        if (text.Length <= maxLength) return text;

        if (maxLength <= 0) return "";

        return text.Substring(0, maxLength);
    }
}


namespace SystemLibrary.Common.Net.Global
{

    /// <summary>
    /// Extension methods for strings
    /// 
    /// They are living in the Global namespace, so they are always available anyhwere in your code
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the first of the parameters sent to this method that is not null, blank or only a single space.
        /// If current, and all 'optional' fallback values does not meet the condition, "" is returned, never null
        /// </summary>
        /// <returns>First non-null, non-empty and non-space string value, or empty string. Returns never null.</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text1 = null;
        /// var text2 = "";
        /// var text3 = " ";
        /// var text4 = "hello";
        /// 
        /// var result = text1.AsFallback(text2, text3, text4);
        /// //result is "hello" as the others are blank/empty
        /// </code>
        /// </example>
        public static string AsFallback(this string current, params string[] fallbacks)
        {
            if (current.Is()) return current;

            if (fallbacks == null || fallbacks.Length == 0) return "";

            foreach (var fallback in fallbacks)
                if (!fallback.IsNot())
                    return fallback;

            return "";
        }

        /// <summary>
        /// Convert a string value to the Enum of your choice
        /// </summary>
        /// <typeparam name="T">T must be an Enum</typeparam>
        /// <param name="value">Value must match the Key or the 'EnumValueAttribute' of a Key in the Enum, else this returns default of the Enum T</param>
        /// <returns>Returns first matching Key or default of the Enum</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// enum EnumColor
        /// {
        ///     [EnumText("White")]
        ///     [EnumValue("BlackAndWhite")]
        ///     Black,
        ///     
        ///     Pink
        /// }
        ///
        /// var value = "black".ToEnum&lt;EnumColor&gt;();
        /// //value is EnumColor.Black, case insensitive conversion
        /// 
        /// var value = "white".ToEnum&lt;EnumColor&gt;();
        /// //value is EnumColor.Black, case insensitive conversion and enum text match
        /// 
        /// var value = "blackAndWhite".ToEnum&lt;EnumColor&gt;();
        /// //value is EnumColor.Black, case insensitive conversion and enum value match
        /// 
        /// var value = "brown".ToEnum&lt;EnumColor&gt;();
        /// //value is EnumColor.Black, no match, returns first/default
        /// </code>
        /// </example>
        public static T ToEnum<T>(this string value) where T : struct, IComparable, IFormattable, IConvertible
        {
            T result;
            var type = typeof(T);

            if (type.IsEnum)
            {
                var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

                if (members?.Length > 0)
                {
                    value = value?.ToLower();

                    foreach (var enumKey in members)
                    {
                        if (enumKey.GetCustomAttribute(SystemType.EnumValueAttributeType) is EnumValueAttribute enumValueAttribute)
                        {
                            if (enumValueAttribute != null && enumValueAttribute.Value?.ToLower() == value)
                                if (Enum.TryParse(enumKey.Name, out result))
                                    return result;
                        }

                        if (enumKey.GetCustomAttribute(SystemType.EnumTextAttributeType) is EnumTextAttribute enumTextAttribute)
                        {
                            if (enumTextAttribute != null && enumTextAttribute.Text?.ToLower() == value)
                                if (Enum.TryParse(enumKey.Name, out result))
                                    return result;
                        }
                    }
                }
            }

            if (value.IsNot()) return default;

            if (Enum.TryParse(value, true, out result))
                return result;

            return result;
        }

        /// <summary>
        /// Check if a text strats with any of the values, case sensitive
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text = "hello world";
        /// 
        /// var result = text.StartsWithAny("", "abcdef", "hel");
        /// //result is true, due to the text begins with 'hel'
        /// </code>
        /// </example>
        public static bool StartsWithAny(this string text, params string[] values)
        {
            if (text.IsNot()) return false;

            if (values.IsNot()) return false;

            for (int i = 0; i < values.Length; i++)
                if (text.StartsWith(values[i]))
                    return true;

            return false;
        }

        /// <summary>
        /// Check if text ends with any of the value, case sensitive
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text = "hello world";
        /// var result = text.EndsWithAny("", "abdef", "rld");
        /// //result is true, because the last part of text ends with rld
        /// </code>
        /// </example>
        public static bool EndsWithAny(this string text, params string[] values)
        {
            if (text.IsNot()) return false;
            if (values.IsNot()) return false;

            if (values.Any(value => text.EndsWith(value)))
                return true;

            return false;
        }

        /// <summary>
        /// Check if text is any of the values, case sensitive
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text = "hello world";
        /// var result = text.IsAny("hello", "world", "hello WORLD", "hello world");
        /// //result is true, as the last 'hello world' matches exactly
        /// </code>
        /// </example>
        public static bool IsAny(this string text, params string[] values)
        {
            if (text.IsNot()) return false;

            if (values.IsNot()) return false;

            for (int i = 0; i < values.Length; i++)
                if (text == values[i])
                    return true;

            return false;
        }

        /// <summary>
        /// Check if text is null, "" or " ", returns true if so, otherwise false
        /// 
        /// Note: it does not check if it contains multiple spaces so it is not exactly as string.IsNullOrWhiteSpace()
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// //Old way: string.IsNullOrWhiteSpace(text);
        /// 
        /// var text = " ";
        /// var result = text.IsNot();
        /// //result is true because a single space counts as "no text" in this function
        /// 
        /// var text = "  "; //2 spaces
        /// var result = text.IsNot();
        /// //result is false because two spaces counts as text in this function
        /// </code>
        /// </example>
        public static bool IsNot(this string text)
        {
            return text == null || text == "" || text == " ";
        }

        /// <summary>
        /// Check if text is not null, not "", and not a " ", returns true if so, otherwise false
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// //Old way: string.IsNullOrWhiteSpace(text);
        /// 
        /// var text = "hello world";
        /// var result = text.Is();
        /// //result is true because text is set to something, and not just "" or " "
        /// </code>
        /// </example>
        public static bool Is(this string text)
        {
            return !text.IsNot();
        }

        /// <summary>
        /// Returns a new string where all non-digit and non-letters has been removed
        /// </summary>
        /// <returns>String with digits and letters only or "", never null</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var email = "support@system.library.com";
        /// var text = email.ToLetterAndDigits();
        /// 
        /// //text is "supportsystemlibrarycom"
        /// </code>
        /// </example>
        public static string ToLetterAndDigits(this string text)
        {
            if (text.IsNot()) return "";

            return new string(text.Where(x => char.IsLetterOrDigit(x)).ToArray());
        }

        /// <summary>
        /// Check if text contains any of the values, case sensitive
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text = "hello";
        /// 
        /// var result = text.ContainsAny("ll");
        /// //result is true, because ll is part of the text
        /// </code>
        /// </example>
        public static bool ContainsAny(this string text, params string[] values)
        {
            if (text.IsNot()) return false;
            if (values.IsNot()) return false;

            if (values.Any(value => text.Contains(value)))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if text contains any of the values, if text ends with one of the value, that values is deleted from the text and then remaning text is returned
        /// 
        /// This is not recursive, so after removal of 1 value, it will return
        /// </summary>
        /// <returns>Returns the input string as is or without one of the values</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var result = RemoveIfEndsWith("abcd", " ", "!", "c", "d");
        /// 
        /// //result is abc
        /// </code>
        /// </example>
        public static string RemoveIfEndsWith(this string text, params string[] values)
        {
            if (text.IsNot()) return text;
            if (values.IsNot()) return text;

            int start = 0;
            int valueLength;
            bool found = false;
            for (int i = 0; i < values.Length; i++)
            {
                valueLength = values[i].Length;
                start = text.Length - valueLength;

                for (int j = 0; j < valueLength; j++)
                {
                    if (text[start + j] != values[i][j])
                        break;

                    if (j == valueLength - 1)
                        found = true;
                }

                if (found)
                    break;
            }

            if (found)
                return text.Substring(0, start);

            return text;
        }

        /// <summary>
        /// Check if text contains any of the characters, case insensitive
        /// </summary>
        /// <param name="characters">Each character in this string will be checked</param>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text = "hello world";
        /// 
        /// var result = text.EndsWithAnyCharacter("abcdef");
        /// //result is true, because it ends with 'd'
        /// </code>
        /// </example>
        public static bool EndsWithAnyCharacter(this string text, string characters)
        {
            if (text.IsNot()) return true;

            if (characters.IsNot()) return true;

            foreach (var character in characters)
            {
                if (text.EndsWith(character.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the string or a substring of the string, based on max allowed string length
        /// </summary>
        /// <returns>Returns the string as it is if text is blank/null or shorter than maxLength, else returns a substring with a length of 'maxLength'. If maxLength is negative it returns ""</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text = "hello world";
        /// var result = text.MaxLength(1);
        /// //result is "h" as it only can be 1 character long
        /// </code>
        /// </example> 
        public static string MaxLength(this string text, int maxLength)
        {
            if (text.IsNot()) return text;

            if (text.Length <= maxLength) return text;

            if (maxLength <= 0) return "";

            return text.Substring(0, maxLength);
        }
    }
}