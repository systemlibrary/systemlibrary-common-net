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
        /// 
        /// <code class="language-csharp hljs">
        /// public enum Colors
        /// {
        ///     Black, White, Red, Blue
        /// }
        /// 
        /// var texts = new object[] { "Red", "Black", "White" };
        /// var colors = texts.AsEnumArray&lt;Colors&gt;();
        /// //colors[1] == Black
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

            throw new Exception("Not supporing conversion of " + type.Name + "-array to the Enum");
        }

        /// <summary>
        /// Convert object to json
        /// 
        /// Default options are: 
        /// - case insensitive
        /// - allows trailing commas
        /// - camel cased
        /// 
        /// Returns a json formatted string representation of the object or null if object is null
        /// </summary>
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

            options = PartialJsonSearcher.Default(options);

            return JsonSerializer.Serialize(obj, options);
        }
    }
}
