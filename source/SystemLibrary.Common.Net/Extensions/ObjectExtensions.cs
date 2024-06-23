using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net.Extensions;

/// <summary>
/// This class contains extension methods on Object
/// 
/// For instance: AsEnum(), AsEnumArray(), etc...
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Convert multiple objects of the same 'enum value' to an array of that enum type
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// public enum Colors
    /// {
    ///     Black, White, Red, Blue
    /// }
    /// 
    /// var integers = new object[] { 1, 2, 3, 4 };
    /// 
    /// var colors = integers.AsEnumArray&lt;Colors&gt;();
    /// // colors is now an array of 'Colors', with one of each of the values:
    /// // colors[0] == Black
    /// // colors[1] == White
    /// // ...
    /// // colors[3] == Blue
    /// </code>
    /// 
    /// <code class="language-csharp hljs">
    /// public enum Colors
    /// {
    ///     Black, White, Red, Blue
    /// }
    /// 
    /// var texts = new object[] { "Red", "Black", "White" };
    /// var colors = texts.AsEnumArray&lt;Colors&gt;();
    /// // colors[1] == Black
    /// </code>
    /// </example>
    public static TEnum[] AsEnumArray<TEnum>(this object[] objects) where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        if (objects.IsNot()) return default;

        //TODO: consider "ToString()" each object, and "ToEnum<TEnum>()" each string...
        //but that would be a new method: ToEnumArray()

        var type = objects[0].GetType();

        if (type == SystemType.IntType)
            return objects.Cast<TEnum>().ToArray();
        else if (type == SystemType.StringType)
            return objects.Select(x => x.ToString().ToEnum<TEnum>()).ToArray();

        throw new Exception("Not supporting conversion of " + type.Name + "-array to the Enum");
    }

    /// <summary>
    /// Convert object to json with option to camelCase C# properties when converting to a json-string
    /// 
    /// Returns a json formatted string representation of the object or null if object is null
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// // Assume camelCase argument true:
    /// class User {
    ///     public string FirstName { get;set; }
    /// }
    /// 
    /// var user = new User();
    /// user.FirstName = "Hello World";
    /// var json = user.Json();
    /// var contains = json.Contains("firstName") &amp;&amp; json.Contains("Hello World"); 
    /// // contains is True, note that Json() has formatted 'FirstName' to 'firstName' when going from C# model to json string
    /// </code>
    /// </example>
    public static string Json(this object obj, bool camelCase)
    {
        if (obj == null) return null;

        var options = GetJsonSerializerOptions.Default(null);

        if (camelCase)
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

        return JsonSerializer.Serialize(obj, options).TranslateUnicodeCodepoints();
    }

    /// <summary>
    /// Convert object to json with option to pass custom JsonConverters which takes precedence over the built-in ones
    /// 
    /// Returns a json formatted string representation of the object or null if object is null
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class User {
    ///     public string FirstName { get;set; }
    /// }
    /// 
    /// class CustomConverter : JsonConverter...
    /// 
    /// var user = new User();
    /// user.FirstName = "Hello World";
    /// var json = user.Json(new CustomConverter());
    /// var isTrue = json.Contains("FirstName") &amp;&amp; json.Contains("Hello World"); 
    /// // isTrue is 'True' as FirstName is not camelCased by default and 'Hello World' is its value
    /// </code>
    /// </example>
    public static string Json(this object obj, JsonSerializerOptions options = null, bool translateUnicodeCodepoints = false, params JsonConverter[] jsonConverters)
    {
        if (obj == null) return null;

        options = GetJsonSerializerOptions.Default(options, jsonConverters);

        if(!translateUnicodeCodepoints)
            return JsonSerializer.Serialize(obj, options);

        return JsonSerializer.Serialize(obj, options).TranslateUnicodeCodepoints();
    }

    /// <summary>
    /// Convert object to json with option to pass custom JsonConverters which takes precedence over the built-in ones
    /// 
    /// Returns a json formatted string representation of the object or null if object is null
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class User {
    ///     public string FirstName { get;set; }
    /// }
    /// 
    /// class CustomConverter : JsonConverter...
    /// 
    /// var user = new User();
    /// user.FirstName = "Hello World";
    /// var json = user.Json(new CustomConverter());
    /// var isTrue = json.Contains("FirstName") &amp;&amp; json.Contains("Hello World"); 
    /// // isTrue is 'True' as FirstName is not camelCased by default and 'Hello World' is its value
    /// </code>
    /// </example>
    public static string Json(this object obj, params JsonConverter[] jsonConverters)
    {
        if (obj == null) return null;

        var options = GetJsonSerializerOptions.Default(null, jsonConverters);

        return JsonSerializer.Serialize(obj, options);
    }
}
