//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Reflection.Metadata.Ecma335;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//using SystemLibrary.Common.Net;
//using SystemLibrary.Common.Net.Attributes;
//using SystemLibrary.Common.Net.Extensions;

///// <summary>
///// This class contains extension methods for Strings
///// 
///// StringExtensions exists in the global namespace
///// </summary>
///// <example>
///// <code>
///// var result = "Hello world".Is()
///// // result is 'true'
///// </code>
///// 
///// <code>
///// var result = "".IsNot();
///// // result is 'true'
///// </code>
///// </example>
//public static class StringExtensions
//{
//    /// <summary>
//    /// Returns 'data', or first non-null and non-blank fallback, if text is null or empty.
//    /// If 'data' and all fallbacks are null or empty, this returns "", never null
//    /// </summary>
//    /// <returns>First non-null, non-empty and non-space string value, or empty string, never null.</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text1 = null;
//    /// var text2 = "";
//    /// var text3 = " ";
//    /// var text4 = "hello";
//    /// 
//    /// var result = text1.OrFirstOf(text2, text3, text4);
//    /// // result is "hello" as the others are blank/empty
//    /// </code>
//    /// </example>
//    public static string OrFirstOf(this string text, params string[] fallbacks)
//    {
//        if (text.Is()) return text;

//        if (fallbacks == null || fallbacks.Length == 0) return "";

//        foreach (var fallback in fallbacks)
//            if (fallback.Is())
//                return fallback;

//        return "";
//    }

//    // <inheritdoc cref="UriExtensions.GetPrimaryDomain"/>
//    /// <summary>
//    /// Returns the domain part of the uri or blank, never null:
//    /// 
//    /// https://www.sub1.sub2.domain.com => domain.com
//    /// </summary>
//    /// <returns>Primary domain name or "", never null</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var result = new Uri('https://systemlibrary.com/image?q=90&amp;format=jpg').GetPrimaryDomain();
//    /// // result is "systemlibrary.com"
//    /// 
//    /// var result = new Uri('https://systemlibrary.github.io/systemlibrary-common-net/image?q=90&amp;format=jpg').GetPrimaryDomain();
//    /// // result is "github.io"
//    /// </code>
//    /// </example>
//    public static string GetPrimaryDomain(this string url)
//    {
//        if (url == null || url.Length == 0) return "";

//        if (url.Contains(" ")) return "";

//        Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);

//        return uri.GetPrimaryDomain();
//    }

//    /// <summary>
//    /// Returns a new string where all 'old values' are replaced with the 'newValue'
//    /// 
//    /// Returns null or blank if such a text is passed in, does not throw exception
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var text = "Hello world 12345";
//    /// 
//    /// var result = text.ReplaceAllWith("A", "Hello", "World", "123", "45");
//    /// // result == A A AA, all mathing texts are replaces with the first param 'A'
//    /// </code>
//    /// </example>
//    public static string ReplaceAllWith(this string text, string newValue, params string[] oldValues)
//    {
//        if (text.IsEmpty()) return text;

//        if (newValue.IsEmpty()) return text;

//        if (oldValues.IsNot()) return text;

//        if (oldValues.Length > 1)
//        {
//            StringBuilder sb = new StringBuilder(text);
//            foreach (var oldValue in oldValues)
//                sb.Replace(oldValue, newValue);

//            return sb.ToString();
//        }

//        return text.Replace(oldValues[0], newValue);
//    }

//    /// <summary>
//    /// Convert a string value to Enum
//    /// </summary>
//    /// <typeparam name="T">T must be an Enum</typeparam>
//    /// <param name="text">Value must match the Key or the 'EnumValueAttribute' or 'EnumTextAttribute' of a Key in the Enum (EnumValue is checked before EnumText), else this returns default of the Enum T</param>
//    /// <returns>Returns first matching Key or default of the Enum</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// enum EnumColor
//    /// {
//    ///     None,
//    ///     
//    ///     [EnumText("White")]
//    ///     [EnumValue("BlackAndWhite")]
//    ///     Black,
//    ///     
//    ///     Pink
//    /// }
//    ///
//    /// var value = "black".ToEnum&lt;EnumColor&gt;();
//    /// // value is EnumColor.Black, case insensitive match directly in the Enum Key (or name if you prefer)
//    /// 
//    /// var value = "white".ToEnum&lt;EnumColor&gt;();
//    /// // value is EnumColor.Black, case insensitive match in 'EnumText' attribute
//    /// 
//    /// var value = "blackAndWhite".ToEnum&lt;EnumColor&gt;();
//    /// // value is EnumColor.Black, case insensitive match in 'EnumValue' attribute
//    /// 
//    /// var value = "brown".ToEnum&lt;EnumColor&gt;();
//    /// // value is EnumColor.None, no match, returns first enum Key
//    /// </code>
//    /// </example>
//    public static T ToEnum<T>(this string text) where T : struct, IComparable, IFormattable, IConvertible
//    {
//        T result;
//        var type = typeof(T);

//        if (type.IsEnum)
//        {
//            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

//            if (members?.Length > 0)
//            {
//                text = text?.ToLower();

//                foreach (var enumKey in members)
//                {
//                    if (enumKey.GetCustomAttribute(SystemType.EnumValueAttributeType) is EnumValueAttribute enumValueAttribute)
//                    {
//                        if (enumValueAttribute != null && enumValueAttribute.Value != null && (enumValueAttribute.Value + "").ToLower() == text)
//                            if (Enum.TryParse(enumKey.Name, out result))
//                                return result;
//                    }

//                    if (enumKey.GetCustomAttribute(SystemType.EnumTextAttributeType) is EnumTextAttribute enumTextAttribute)
//                    {
//                        if (enumTextAttribute != null && enumTextAttribute.Text?.ToLower() == text)
//                            if (Enum.TryParse(enumKey.Name, out result))
//                                return result;
//                    }
//                }
//            }
//        }

//        if (text.IsNot()) return default;

//        if (Enum.TryParse(text, true, out result))
//            return result;

//        return result;
//    }

//    /// <summary>
//    /// Returns true if text starts with any of the values, case sensitive
//    /// </summary>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text = "hello world";
//    /// 
//    /// var result = text.StartsWithAny("", "abcdef", "hel");
//    /// // result is true, due to the text begins with 'hel'
//    /// </code>
//    /// </example>
//    public static bool StartsWithAny(this string text, params string[] values)
//    {
//        if (text.IsNot()) return false;

//        if (values.IsNot()) return false;

//        var textSpan = text.AsSpan();

//        for (int i = 0; i < values.Length; i++)
//            if (values[i].Length != 0 && textSpan.StartsWith(values[i]))
//                return true;

//        return false;
//    }

//    /// <summary>
//    /// Returns true if text ends with any of the values, case sensitive
//    /// </summary>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text = "hello world";
//    /// var result = text.EndsWithAny("", "abdef", "rld");
//    /// // result is true, because the last part of text ends with rld
//    /// </code>
//    /// </example>
//    public static bool EndsWithAny(this string text, params string[] values)
//    {
//        if (text.IsNot()) return false;
//        if (values.IsNot()) return false;

//        var textSpan = text.AsSpan();

//        for (int i = 0; i < values.Length; i++)
//            if (values[i].Length != 0 && textSpan.EndsWith(values[i]))
//                return true;

//        return false;
//    }

//    /// <summary>
//    /// Returns true if text ends with any of the values, case insensitive
//    /// </summary>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text = "heLLo WoRLd";
//    /// var result = text.EndsWithAny("", "abdef", "RLD");
//    /// // result is true, because the last part of text ends with RLd - case insensitive
//    /// </code>
//    /// </example>
//    public static bool EndsWithAnyCaseInsensitive(this string text, params string[] values)
//    {
//        if (text.IsNot()) return false;
//        if (values.IsNot()) return false;

//        var textSpan = text.ToLower().AsSpan();

//        for (int i = 0; i < values.Length; i++)
//            if (values[i].Length != 0 && textSpan.EndsWith(values[i].ToLower()))
//                return true;

//        return false;
//    }

//    /// <summary>
//    /// Returns true if text equals any of the values, case sensitive
//    /// </summary>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text = "hello world";
//    /// var result = text.IsAny("hello", "world", "hello WORLD", "hello world");
//    /// // result is true, as the last 'hello world' matches exactly
//    /// </code>
//    /// </example>
//    public static bool IsAny(this string text, params string[] values)
//    {
//        if (text.IsNot()) return false;

//        if (values.IsNot()) return false;

//        for (int i = 0; i < values.Length; i++)
//            if (text == values[i])
//                return true;

//        return false;
//    }

//    /// <summary>
//    /// Returns true if text is null, "" or " ", else false
//    /// 
//    /// Note: it does not check if it contains multiple spaces so it is not exactly as string.IsNullOrWhiteSpace()
//    /// </summary>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// //Old way: string.IsNullOrWhiteSpace(text);
//    /// 
//    /// var text = " ";
//    /// var result = text.IsNot();
//    /// // result is true because a single space counts as "no text" in this function
//    /// 
//    /// var text = "  "; //2 spaces
//    /// var result = text.IsNot();
//    /// // result is false because two spaces counts as text in this function
//    /// </code>
//    /// </example>
//    public static bool IsNot(this string text, params string[] additionalNotValues)
//    {
//        if (text == null || text.Length == 0) return true;

//        if (text.Length == 1 && text == " ") return true;

//        if (additionalNotValues == null) return false;

//        if (additionalNotValues.Any(value => text == value))
//            return true;

//        return false;
//    }

//    /// <summary>
//    /// Returns true if text is null or has a length of 0 (empty string), else false
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var text = "Hello world";
//    /// var isEmpty = text.IsEmpty(); 
//    /// // isEmpty is false
//    /// 
//    /// var text = ""; //or null
//    /// var isEmpty = text.IsEmpty();
//    /// // isEmpty is true
//    /// 
//    /// var text = " "; //a space
//    /// var isEmpty = text.IsEmpty();
//    /// // isEmpty is false
//    /// </code>
//    /// </example>
//    public static bool IsEmpty(this string text)
//    {
//        return text == null || text.Length == 0;
//    }

//    /// <summary>
//    /// Returns true if text is not null and not "" and not  " ", else false
//    /// </summary>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// // Old way: string.IsNullOrWhiteSpace(text);
//    /// 
//    /// var text = "hello world";
//    /// var result = text.Is();
//    /// // result is true because text is set to something, and not just "" or " "
//    /// </code>
//    /// </example>
//    public static bool Is(this string text)
//    {
//        if (text == null || text.Length == 0) return false;

//        return !(text.Length == 1 && text == " ");
//    }

//    /// <summary>
//    /// Returns true if text is not null and not "" and not  " " and not any of the 'invalidTexts', else false
//    /// 
//    /// Case sensitive
//    /// </summary>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text = "hello world";
//    /// var result = text.Is("hello");
//    /// // result is true because text is set to something else than 'hello', and not just "" or " "
//    /// 
//    /// var result2 = text.Is("hello world");
//    /// // result is false, because text equals to the invalid text passed in, which was 'hello world'
//    /// </code>
//    /// </example>
//    public static bool Is(this string text, params string[] invalidTexts)
//    {
//        if (text.IsNot())
//            return false;

//        if (invalidTexts == null || invalidTexts.Length == 0)
//            return true;

//        foreach (var word in invalidTexts)
//            if (text == word)
//                return false;

//        return true;
//    }

//    /// <summary>
//    /// Returns new string without all non-digit and non-letters
//    /// </summary>
//    /// <returns>String with digits and letters only or "", never null</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var email = "support@system.library.com";
//    /// var text = email.ToLetterAndDigits();
//    /// 
//    /// // text is "supportsystemlibrarycom"
//    /// </code>
//    /// </example>
//    public static string ToLetterAndDigits(this string text)
//    {
//        if (text.IsNot()) return "";

//        return new string(text.Where(x => char.IsLetterOrDigit(x)).ToArray());
//    }

//    /// <summary>
//    /// Returns true if text contains any of the values, case sensitive
//    /// </summary>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text = "hello";
//    /// 
//    /// var result = text.ContainsAny("123", "!", "lo");
//    /// // result is true, because lo is part of the text
//    /// </code>
//    /// </example>
//    public static bool ContainsAny(this string text, params string[] values)
//    {
//        if (text.IsNot()) return false;

//        if (values.IsNot()) return false;

//        return values.Any(x => text.Contains(x));
//    }

//    /// <summary>
//    /// Returns trimmed version of the text input, if text ends with any of the values, case sensitive
//    /// 
//    /// This is not recursive, so after removal of 1 value, it will return
//    /// 
//    /// NOTE: Works like "".TrimEnd(), but with multiple strings in one go
//    /// 
//    /// NOTE: It does not implicitly trim spaces, unless you pass spaces as one of the values
//    /// </summary>
//    /// <returns>Returns the input string as is or without one of the values passed</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var result = "abcd".TrimEnd(" ", "!", "c", "d");
//    /// 
//    /// // result is abc
//    /// 
//    /// var result = "abcd".TrimEnd(" ", "d", "bc");
//    /// 
//    /// // result is "abc" because it matches 'd' then returns, so 'bc' is never checked
//    /// </code>
//    /// </example>
//    public static string TrimEnd(this string text, params string[] values)
//    {
//        if (text.IsNot()) return text;
//        if (values.IsNot()) return text;

//        int start = 0;
//        int valueLength;
//        bool found = false;

//        var textSpan = text.AsSpan();

//        for (int i = 0; i < values.Length; i++)
//        {
//            valueLength = values[i].Length;
//            start = text.Length - valueLength;

//            for (int j = 0; j < valueLength; j++)
//            {
//                if (textSpan[start + j] != values[i][j])
//                    break;

//                if (j == valueLength - 1)
//                    found = true;
//            }

//            if (found)
//                break;
//        }

//        if (found)
//            return textSpan.Slice(0, start).ToString();

//        return textSpan.ToString();
//    }

//    /// <summary>
//    /// Returns true if text ends with any of the characters, case insensitive
//    /// 
//    /// </summary>
//    /// <param name="characters">Each character in this string will be checked</param>
//    /// <returns>True or false</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text = "hello world";
//    /// 
//    /// var result = text.EndsWithAnyCharacter("abcdef");
//    /// // result is true, because it ends with 'd'
//    /// </code>
//    /// </example>
//    public static bool EndsWithAnyCharacter(this string text, string characters)
//    {
//        if (text.IsNot()) return true;

//        if (characters.IsNot()) return true;

//        var textSpan = text.AsSpan();
//        var span = characters.AsSpan();

//        foreach (var character in span)
//        {
//            if (textSpan.EndsWith(character.ToString(), StringComparison.InvariantCultureIgnoreCase))
//                return true;
//        }

//        return false;
//    }

//    /// <summary>
//    /// Returns the string or a substring of the string, based on max allowed string length
//    /// </summary>
//    /// <returns>Returns the string as it is if text is blank/null or shorter than maxLength, else returns a substring with a length of 'maxLength'. If maxLength is negative it returns ""</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// var text = "hello world";
//    /// var result = text.MaxLength(1);
//    /// // result is "h" as it only can be 1 character long
//    /// </code>
//    /// </example> 
//    public static string MaxLength(this string text, int maxLength)
//    {
//        if (text == null) return "";

//        if (text.Length <= maxLength) return text;

//        if (maxLength <= 0) return "";

//        return new string(text.AsSpan(0, maxLength));
//    }

//    /// <summary>
//    /// Return a part of the json as T
//    /// 
//    /// Searches through the json formatted text to find the property it takes as input, and outputs T
//    /// 
//    /// Supports a 'search path' seperated by a forward slash to the leaf property you want to convert to T
//    /// 
//    /// Searching for a property by name is case-insensitive
//    /// 
//    /// Throws exception if the json formatted text is invalid or a parent property to the leaf do not exist in the json text
//    /// 
//    /// Returns T or null if the leaf property do not exist
//    /// </summary>
//    /// <typeparam name="T">A class or list/array of a class
//    /// 
//    /// If T is a list or array and no 'findPropertySearchPath' is specified, the Searcher appends an 's' as suffix
//    /// 
//    /// For instance List&lt;User&gt; will search for a property 'users', case insensitive and 's' is appended
//    /// </typeparam>
//    /// <param name="json">Json formatted string</param>
//    /// <param name="findPropertySearchPath">
//    /// Name of the property that will be deserialized as T
//    /// 
//    /// Example: root/property1/property2/leaf where 'leaf' will be deserialized as T
//    /// </param>
//    /// <returns>Returns json as T or null if not found</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// // Assume json string stored in a C# variable named 'data':
//    /// var data = "{
//    ///     "users" [
//    ///         ...
//    ///     ]
//    /// }";
//    /// var users = data.PartialJson&lt;List&lt;User&gt;&gt;();
//    /// // When a 'property name' is not given as first argument, it uses the type name in the following manner:
//    /// // 1. Takes the type name, or generic type name, in our case 'User'
//    /// // 2. If type is a List or Array, it adds a plural 's', so now we have 'Users'
//    /// // 3. It lowers first letter to match camel casing as thats the "norm", so now we have 'users'
//    /// 
//    /// // You could also pass in "users" manually if you wanted, result is the same:
//    /// // var users = data.PartialJson&lt;List&lt;User&gt;&gt;("users");
//    /// 
//    /// // Conclusion: 
//    /// // Automatic finding the property name if not specified as first argument
//    /// // It returns first "users" match in the json, no matter how deep it is
//    /// 
//    /// //Assume json string stored in a C# variable named 'data':
//    /// var data = "{
//    ///     "users" [
//    ///         ...
//    ///     ],
//    ///     "deactivated": {
//    ///         "users": [
//    ///             ...
//    ///         ]
//    ///     }
//    /// }";
//    /// var users = data.PartialJson&lt;List&lt;User&gt;&gt;("deactivated/users");
//    /// // Searches for a property "deactivated" anywhere in the json, then anywhere inside that, a "users" property
//    /// 
//    /// 
//    /// // Assume json string stored in a C# variable named 'data':
//    /// var data = "{
//    ///     "text": "hello world",
//    ///     "employees": [
//    ///         {
//    ///             "hired": [
//    ///                ...
//    ///             ],
//    ///             "fired": [
//    ///                 ...
//    ///             ]
//    ///         }
//    ///     ],
//    /// }";
//    /// 
//    /// var users = data.PartialJson&lt;List&lt;User&gt;&gt;("fired");
//    /// // Searches for a property anywhere in the json named "fired"
//    /// </code>
//    /// </example>
//    public static T PartialJson<T>(this string json, string findPropertySearchPath = null, JsonSerializerOptions options = null) where T : class
//    {
//        return PartialJsonSearcher.Search<T>(json, findPropertySearchPath, options);
//    }

//    /// <summary>
//    /// Convert string formatted json to object T
//    /// 
//    /// Default options are: 
//    /// - case insensitive
//    /// - allows trailing commas
//    /// - camel cased
//    /// 
//    /// Throws exception if json has invalid formatted json text
//    /// </summary>
//    /// <returns>Returns T or null if json is null or empty</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// class User {
//    ///     public string FirstName;
//    ///     public int Age { get; set;}
//    /// }
//    /// var json = "{
//    ///     "firstName": 'hello',
//    ///     "age": 10
//    /// }";
//    /// 
//    /// var user = json.Json&lt;User&gt;();
//    /// </code>
//    /// </example>
//    public static T Json<T>(this string json, JsonSerializerOptions options = null) where T : class
//    {
//        if (json.IsNot()) return default;

//        options = GetJsonSerializerOptions.Default(options);

//        return JsonSerializer.Deserialize<T>(json, options);
//    }

//    /// <summary>
//    /// Convert string formatted json to object T with your additional JsonConverters
//    /// 
//    /// Throws exception if json has invalid formatted json text
//    /// </summary>
//    /// <returns>Returns T or null if json is null or empty</returns>
//    /// <example>
//    /// <code class="language-csharp hljs">
//    /// class User {
//    ///     public string FirstName;
//    ///     public int Age { get; set;}
//    /// }
//    /// 
//    /// class CustomConverter : JsonConverter...
//    /// 
//    /// var json = "{
//    ///     "firstName": 'hello',
//    ///     "age": 10
//    /// }";
//    /// 
//    /// var user = json.Json&lt;User&gt;(new CustomConverter());
//    /// </code>
//    /// </example>
//    public static T Json<T>(this string json, params JsonConverter[] converters) where T : class
//    {
//        if (json.IsNot()) return null;

//        var options = GetJsonSerializerOptions.Default(null, converters);

//        return JsonSerializer.Deserialize<T>(json, options);
//    }

//    /// <summary>
//    /// Darken or lighten a hex value by a factor
//    /// 
//    /// - pass a positive factor to darken
//    /// - pass a negative factor to lighten
//    /// 
//    /// - factor is a number between 0 and 1
//    /// 
//    /// - pass auto: true, to automatically check difference in the new value, and if it is too small, the value is rather darkened instead of lightened, or ligtened instead of darkened
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var value = "#FFF";
//    /// var newValue = value.HexDarkenOrLighten();
//    /// // newValue is #4F4F4F
//    /// </code>
//    /// </example>
//    public static string HexDarkenOrLighten(this string hex, double factor = 0.31, bool auto = false)
//    {
//        if (hex.IsNot()) return hex;

//        var hasHex = hex.StartsWith("#");
//        if (hasHex)
//            hex = hex.Substring(1);

//        if (hex.Length != 6 && hex.Length != 3)
//            throw new Exception("Hex is out of range, must be either 6 or 3, like: #FFF or #000000");

//        int partLength = hex.Length == 6 ? 2 : 1;

//        var color = new StringBuilder("");

//        IEnumerable<string> SplitHex(string hex, int partLength)
//        {
//            for (var i = 0; i < hex.Length; i += partLength)
//                yield return hex.Substring(i, Math.Min(partLength, hex.Length - i));
//        }

//        double colorValue;

//        var minDiff = 55;
//        foreach (var part in SplitHex(hex, partLength))
//        {
//            var number = Convert.ToInt32(part, 16);

//            if (factor < 0)
//            {
//                colorValue = number - number * factor;

//                if (auto && colorValue - number <= minDiff)
//                {
//                    if (colorValue <= 128)
//                        colorValue = 255 - number;
//                    else
//                        colorValue = colorValue - number;
//                }

//                if (colorValue > 255)
//                    colorValue = colorValue - 255;
//            }
//            else
//            {
//                colorValue = number * factor;

//                if (auto && number - colorValue <= minDiff)
//                {
//                    if (colorValue <= 128)
//                        colorValue = 255 - colorValue;
//                    else
//                        colorValue = number - colorValue;
//                }
//                if (colorValue > 255)
//                    colorValue = colorValue - 255;

//            }

//            var temp = Convert.ToInt32(colorValue).ToString("X");

//            if (temp.Length == 1)
//                temp = "0" + temp;

//            color.Append(temp);
//        }

//        if (hasHex)
//            return "#" + color;

//        return color.ToString();
//    }

//    /// <summary>
//    /// Returns input as a base64 string
//    /// 
//    /// Returns null or empty if input is null or empty
//    /// 
//    /// //Tip: If you dont need base64 format, .Obfuscating() method is faster if data is less than ~400KB
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var value = "Hello world";
//    /// var base64string = value.ToBase64();
//    /// </code>
//    /// </example>
//    public static string ToBase64(this string text, Encoding encoding = default)
//    {
//        return GetBytes(text, encoding).ToBase64();
//    }

//    /// <summary>
//    /// Returns the base64string input as a readable string
//    /// 
//    /// Returns null or empty if input is null or empty
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var value = "Hello world";
//    /// var base64string = value.ToBase64();
//    /// var valueAgain = base64string.FromBase64();
//    /// // value == valueAgain
//    /// </code>
//    /// </example>
//    public static string FromBase64(this string base64String, Encoding encoding = default)
//    {
//        if (base64String == null) return null;

//        if (encoding == default)
//            encoding = Encoding.UTF8;

//        return encoding.GetString(Convert.FromBase64String(base64String));
//    }

//    /// <summary>
//    /// Returns a byte array of the input
//    /// 
//    /// Returns null or empty if input is null or empty
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var value = "Hello world";
//    /// var bytes = value.GetBytes();
//    /// </code>
//    /// </example>
//    public static byte[] GetBytes(this string text, Encoding encoding = default)
//    {
//        if (text == null) return null;

//        if (encoding == default)
//            encoding = Encoding.UTF8;

//        return encoding.GetBytes(text);
//    }

//    /// <summary>
//    /// Obfuscate a string to a different string with a salt
//    /// 
//    /// Throws exception if salt is &lt;= 0, salt should be in range from 1 to 65000
//    /// 
//    /// Returns a new obfuscated string
//    /// 
//    /// Returns null or empty if input is null or empty
//    /// 
//    /// // Tip: Method .ToBase64() is faster if data is more than 400KB
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var value = "Hello world";
//    /// var obfuscatedText = value.Obfuscate();
//    /// </code>
//    /// </example>
//    public static string Obfuscate(this string text, int salt = 11)
//    {
//        return SystemLibrary.Common.Net.Obfuscate.Convert(text, salt, false);
//    }

//    /// <summary>
//    /// Deobfuscate a string back to its readable state with a salt
//    /// 
//    /// Returns the text as it was before obfuscating, assuming you used the same salt value
//    /// 
//    /// Returns null or empty if input is null or empty
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var value = "Hello world";
//    /// var obfuscatedText = value.Obfuscate();
//    /// var deobfuscatedText = obfuscatedText.Deobfuscate();
//    /// // value == deobfuscatedText
//    /// </code>
//    /// </example>
//    public static string Deobfuscate(this string text, int salt = 11)
//    {
//        return SystemLibrary.Common.Net.Obfuscate.Convert(text, salt, true);
//    }

//    /// <summary>
//    /// Returns a MD5 hash version of the text input, resulting in a 47 character long text (including dashes), no matter the input, or returns null or empty if that was the input
//    /// 
//    /// Tip: If data is larger than ~200 bytes then .ToSha1Hash() is faster
//    /// 
//    /// Note: Md5 is not secure, there are rainbow tables, it's a 'one way shrinking obfuscater'
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var value = "Hello world";
//    /// var md5text = value.ToMD5Hash();
//    /// </code>
//    /// </example>
//    public static string ToMD5Hash(this string text)
//    {
//        return Md5.Compute(text.GetBytes());
//    }

//    /// <summary>
//    /// Returns a Sha1 hash version of the text input, resulting in a 59 character long text (including dashes), no matter the input, or returns null or empty if that was the input
//    /// 
//    /// Tip: If data is smaller than ~200 bytes then .ToMD5Hash() is faster
//    /// 
//    /// Note: Sha1 is not secure, there are rainbow tables, it's a 'one way shrinking obfuscater'
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var value = "Hello world";
//    /// var sha1 = value.ToSha1Hash();
//    /// </code>
//    /// </example>
//    public static string ToSha1Hash(this string text)
//    {
//        return Sha1.Compute(text.GetBytes());
//    }
//}
