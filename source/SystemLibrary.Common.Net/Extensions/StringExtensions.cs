using System;
using System.Linq;
using System.Reflection;

using SystemLibrary.Common.Net;
using SystemLibrary.Common.Net.Attributes;
using SystemLibrary.Common.Net.Extensions;

/// <summary>
/// This class contains extension methods for Strings
/// 
/// WARNING: These extension methods are living in the global namespace, so they are available from anywhere as long as you've referenced the Nuget Package (the dll)
/// </summary>
/// <example>
/// var result = "Hello world".Is() //invokable from anywhere in your applications source code
/// //result is 'true'
/// </example>
public static class StringExtensions
{
    /// <summary>
    /// Returns 'data', or first non-null and non-blank fallback, if text is null or empty.
    /// If 'data' and all fallbacks are null or empty, this returns "", never null
    /// </summary>
    /// <returns>First non-null, non-empty and non-space string value, or empty string, never null.</returns>
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
    public static string AsFallback(this string data, params string[] fallbacks)
    {
        if (data.Is()) return data;

        if (fallbacks == null || fallbacks.Length == 0) return "";

        foreach (var fallback in fallbacks)
            if (fallback.Is())
                return fallback;

        return "";
    }

    /// <summary>
    /// Returns 'primary domain' from the url as input
    /// </summary>
    /// <returns>Returns primary domain, 'localhost' from url: 'https://localhost.com/image.png?q=90' or empty string, never null</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var result = "https://localhost.com/image.png?q=90".GetPrimaryDomain();
    /// //result is "localhost"
    /// </code>
    /// </example>
    public static string GetPrimaryDomain(this string url)
    {
        if (url.IsNot()) return "";

        Uri uri = new Uri(url);

        return uri.GetPrimaryDomain();
    }

    /// <summary>
    /// Convert a string value to Enum
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
    /// Returns true if text starts with any of the values, case sensitive
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
    /// Returns true if text ends with any of the values, case sensitive
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
    /// Returns true if text ends with any of the values, case insensitive
    /// </summary>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = "heLLo WoRLd";
    /// var result = text.EndsWithAny("", "abdef", "RLD");
    /// //result is true, because the last part of text ends with RLd - case insensitive
    /// </code>
    /// </example>
    public static bool EndsWithAnyCaseInsensitive(this string text, params string[] values)
    {
        if (text.IsNot()) return false;
        if (values.IsNot()) return false;

        text = text.ToLower();

        if (values.Any(value => text.EndsWith(value.ToLower())))
            return true;

        return false;
    }

    /// <summary>
    /// Returns true if text equals any of the values, case sensitive
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
    /// Returns true if text is null, "" or " ", else false
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
    /// Returns true if text is not null, nor "", nor a " ", else false
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
        return text != null && text != "" && text != " ";
    }

    /// <summary>
    /// Returns new string without all non-digit and non-letters
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
    /// Returns true if text contains any of the values, case sensitive
    /// </summary>
    /// <returns>True or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = "hello";
    /// 
    /// var result = text.ContainsAny("123", "!", "lo");
    /// //result is true, because lo is part of the text
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
    /// Returns trimmed version of the text input, if text ends with any of the values, case sensitive
    /// 
    /// This is not recursive, so after removal of 1 value, it will return
    /// 
    /// NOTE: Works like "".TrimEnd(), but with multiple strings in one go
    /// </summary>
    /// <returns>Returns the input string as is or without one of the values passed</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var result = "abcd".TrimEnd(" ", "!", "c", "d");
    /// 
    /// //result is abc
    /// 
    /// var result = "abcd".TrimEnd(" ", "d", "bc");
    /// 
    /// //result is "abc" because it matches 'd' then returns, so 'bc' is never checked
    /// </code>
    /// </example>
    public static string TrimEnd(this string text, params string[] values)
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
    /// Returns true if text ends with any of the characters, case insensitive
    /// 
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
    /// This class contains extension methods for Strings
    /// 
    /// WARNING: These extension methods are living in the global namespace, so they are available from anywhere as long as you've referenced the Nuget Package (the dll)
    /// </summary>
    /// <example>
    /// var result = "Hello world".Is() //invokable from anywhere in your applications source code
    /// //result is 'true'
    /// </example>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns 'data', or first non-null and non-blank fallback, if text is null or empty.
        /// If 'data' and all fallbacks are null or empty, this returns "", never null
        /// </summary>
        /// <returns>First non-null, non-empty and non-space string value, or empty string, never null.</returns>
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
        public static string AsFallback(this string data, params string[] fallbacks)
        {
            if (data.Is()) return data;

            if (fallbacks == null || fallbacks.Length == 0) return "";

            foreach (var fallback in fallbacks)
                if (fallback.Is())
                    return fallback;

            return "";
        }

        /// <summary>
        /// Returns 'primary domain' from the url as input
        /// </summary>
        /// <returns>Returns primary domain, 'localhost' from url: 'https://localhost.com/image.png?q=90' or empty string, never null</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var result = "https://localhost.com/image.png?q=90".GetPrimaryDomain();
        /// //result is "localhost"
        /// </code>
        /// </example>
        public static string GetPrimaryDomain(this string url)
        {
            if (url.IsNot()) return "";

            Uri uri = new Uri(url);

            return uri.GetPrimaryDomain();
        }

        /// <summary>
        /// Convert a string value to Enum
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
        /// Returns true if text starts with any of the values, case sensitive
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
        /// Returns true if text ends with any of the values, case sensitive
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
        /// Returns true if text ends with any of the values, case insensitive
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text = "heLLo WoRLd";
        /// var result = text.EndsWithAny("", "abdef", "RLD");
        /// //result is true, because the last part of text ends with RLd - case insensitive
        /// </code>
        /// </example>
        public static bool EndsWithAnyCaseInsensitive(this string text, params string[] values)
        {
            if (text.IsNot()) return false;
            if (values.IsNot()) return false;

            text = text.ToLower();

            if (values.Any(value => text.EndsWith(value.ToLower())))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if text equals any of the values, case sensitive
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
        /// Returns true if text is null, "" or " ", else false
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
        /// Returns true if text is not null, nor "", nor a " ", else false
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
            return text != null && text != "" && text != " ";
        }

        /// <summary>
        /// Returns new string without all non-digit and non-letters
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
        /// Returns true if text contains any of the values, case sensitive
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var text = "hello";
        /// 
        /// var result = text.ContainsAny("123", "!", "lo");
        /// //result is true, because lo is part of the text
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
        /// Returns trimmed version of the text input, if text ends with any of the values, case sensitive
        /// 
        /// This is not recursive, so after removal of 1 value, it will return
        /// 
        /// NOTE: Works like "".TrimEnd(), but with multiple strings in one go
        /// </summary>
        /// <returns>Returns the input string as is or without one of the values passed</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var result = "abcd".TrimEnd(" ", "!", "c", "d");
        /// 
        /// //result is abc
        /// 
        /// var result = "abcd".TrimEnd(" ", "d", "bc");
        /// 
        /// //result is "abc" because it matches 'd' then returns, so 'bc' is never checked
        /// </code>
        /// </example>
        public static string TrimEnd(this string text, params string[] values)
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
        /// Returns true if text ends with any of the characters, case insensitive
        /// 
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