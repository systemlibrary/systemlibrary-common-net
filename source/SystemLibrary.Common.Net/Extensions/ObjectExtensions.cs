using System;
using System.Linq;

namespace SystemLibrary.Common.Net.Extensions
{
    /// <summary>
    /// Extension methods for objects like AsEnum(), AsEnumArray()...
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
        /// Cast the object array to System.Enum array
        /// </summary>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var integers = new object[] { 1, 2, 3, 4 };
        /// var colors = integers.AsEnumArray&lt;Colors&gt;();
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
