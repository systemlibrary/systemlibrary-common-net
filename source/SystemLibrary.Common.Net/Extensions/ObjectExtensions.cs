using System;
using System.Linq;

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
    }
}
