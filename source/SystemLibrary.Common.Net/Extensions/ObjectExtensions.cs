using System;
using System.Linq;
using System.Text.Json;

namespace SystemLibrary.Common.Net.Extensions
{
    /// <summary>
    /// This class contains extension methods on Object
    /// 
    /// For instance: AsEnum(), AsEnumArray(), etc...
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Cast the object to a System.Enum
        /// </summary>
        /// <example>
        /// <code class="language-csharp">
        /// enum Color { 
        ///     Red
        /// }
        /// 
        /// var color = Color.Red;
        /// 
        /// var colorEnum = color.AsEnum();
        /// //now "color" is System.Enum and can be passed to functions that takes Enum type
        /// </code>
        /// </example>
        public static Enum AsEnum(this object obj)
        {
            if (obj != null)
                return (Enum)obj;

            return default;
        }

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
        /// //colors is now an array of 'Colors', with one of each of the values:
        /// //colors[0] == Black
        /// //colors[1] == White
        /// //...
        /// //colors[3] == Blue
        /// </code>
        /// </example>
        public static TEnum[] AsEnumArray<TEnum>(this object[] objects) where TEnum : IComparable, IFormattable, IConvertible
        {
            if (objects.IsNot()) return default;

            //TODO: consider "ToString()" each object, and "ToEnum<TEnum>()" each string...
            //but that would be a new method: ToEnumArray()..

            return objects.Cast<TEnum>().ToArray();
        }

        /// <summary>
        /// Convert object to json
        /// 
        /// Default options are: 
        /// - case insensitive
        /// - allows trailing commas
        /// - camel cased
        /// 
        /// </summary>
        /// <returns>Returns a json formatted string representation of the object or null if object is null</returns>
        /// <example>
        /// <code>
        /// class User {
        ///     public string FirstName { get;set; }
        /// }
        /// 
        /// var user = new User();
        /// user.FirstName = "Hello World";
        /// var json = user.ToJson();
        /// var contains = json.Contains("firstName") && json.Contains("Hello World"); 
        /// //contains is True, note that ToJson() has formatted 'FirstName' to camelCasing
        /// </code>
        /// </example>
        public static string ToJson(this object obj, JsonSerializerOptions options = null)
        {
            if (obj == null) return null;

            var setReadCommentHandling = options == null;

            options = PartialJsonSearcher.Default(options);

            if (setReadCommentHandling)
                options.ReadCommentHandling = AppSettingsConfig.Current.SystemLibraryCommonNet.Json.ReadCommentHandling;

            return JsonSerializer.Serialize(obj, options);
        }
    }
}
