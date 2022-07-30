using System;
using System.Collections.Generic;
using System.Reflection;

using SystemLibrary.Common.Net.Attributes;

namespace SystemLibrary.Common.Net.Extensions
{
    /// <summary>
    /// This class contains extension methods for Enum as a Generic Type
    /// 
    /// WARNING: Current version of C# does not allow extension methods 'generic types', hence these are static methods
    /// </summary>
    public static class EnumExtensions<TEnum> where TEnum : IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Get all keys in the Enum
        /// </summary>
        /// <returns>All keys in the enum as strings</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var keys = SystemLibrary.Common.Net.Extensions.EnumExtesions&lt;SomeEnum&gt;.GetKeys();
        /// </code>
        /// </example>
        public static IEnumerable<string> GetKeys()
        {
            var type = typeof(TEnum);
            if (!type.IsEnum) throw new Exception("Could not get values from a non-enum object. " + type.Name + " is not an Enum");

            var values = Enum.GetValues(type);

            foreach (var value in values)
            {
                yield return value.ToString();
            }
        }

        /// <summary>
        /// Get all keys as the enum value itself
        /// </summary>
        /// <returns>All keys in the enum as Enums</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var enums = SystemLibrary.Common.Net.Extensions.EnumExtesions&lt;SomeEnum&gt;.GetEnums();
        /// </code>
        /// </example>
        public static IEnumerable<TEnum> GetEnums()
        {
            var type = typeof(TEnum);
            if (!type.IsEnum) throw new Exception("Could not get values from a non-enum object. " + type.Name + " is not an Enum");

            var values = Enum.GetValues(type);

            foreach (var value in values)
            {
                yield return (TEnum)value;
            }
        }
    }

    /// <summary>
    /// Extension methods for Enums like ToText(), ToValue(), IsAny(), ...
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Check if enum is equal to any of the ones in the array
        /// </summary>
        /// <returns>True or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// enum Color {
        ///     Black,
        ///     White,
        ///     Red,
        ///     Blue
        /// }
        /// 
        /// var red = Color.Red;
        /// 
        /// if(red.IsAny(Color.Black, Color.Blue)) {
        ///     //Never hit, red is Red, never blue/black
        /// }
        /// </code>
        /// </example>
        public static bool IsAny(this Enum enumField, params Enum[] values)
        {
            foreach (var value in values)
                if (Equals(enumField, value))
                    return true;

            return false;
        }

        /// <summary>
        /// Gets the EnumText-attribute's value, fallback to enumField.ToString()
        /// 
        /// Can return null if the EnumText-attribute exists and has a value of null
        /// </summary>
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
        /// var value = EnumColor.Black.ToText();
        /// //White
        /// 
        /// var value = EnumColor.Pink.ToText();
        /// //Pink
        /// </code>
        /// </example>
        public static string ToText(this Enum enumField)
        {
            var textAttribute = GetAttribute<EnumTextAttribute>(enumField, SystemType.EnumTextAttributeType);

            if (textAttribute != null)
                return textAttribute.Text;

            return enumField?.ToString();
        }

        /// <summary>
        /// Gets the EnumValue-attribute's object value
        /// 
        /// Returns null if EnumValue attribute do not exist or its value is actually null
        /// </summary>
        /// <example>
        /// <code class="language-csharp hljs">
        /// enum EnumColor
        /// {
        ///     [EnumText("White")]
        ///     [EnumValue(1234)]
        ///     Black,
        ///     
        ///     [EnumText((string)null)]
        ///     Red,
        ///     Pink
        /// }
        ///
        /// var value = EnumColor.Black.GetEnumText();
        /// //"White"
        /// 
        /// var value = EnumColor.Red.GetEnumText();
        /// //null
        /// 
        /// var value = EnumColor.Pink.GetEnumText();
        /// //null
        /// </code>
        /// </example>
        public static string GetEnumText(this Enum enumField)
        {
            var textAttribute = GetAttribute<EnumTextAttribute>(enumField, SystemType.EnumTextAttributeType);

            return textAttribute?.Text;
        }

        /// <summary>
        /// Gets the EnumValue-attribute's value, fallback to enumField.ToString()
        /// 
        /// Cannot return null
        /// </summary>
        /// <example>
        /// <code class="language-csharp hljs">
        /// enum EnumColor
        /// {
        ///     [EnumText("White")]
        ///     [EnumValue(1234)]
        ///     Black,
        ///     
        ///     Pink
        /// }
        ///
        /// var value = EnumColor.Black.ToValue();
        /// //"1234" as string
        /// 
        /// var value = EnumColor.Pink.ToValue();
        /// //Pink
        /// </code>
        /// </example>
        public static string ToValue(this Enum enumField)
        {
            var valueAttribute = GetAttribute<EnumValueAttribute>(enumField, SystemType.EnumValueAttributeType);

            if (valueAttribute != null)
                return valueAttribute.Value + "";

            return enumField + "";
        }

        /// <summary>
        /// Gets the EnumValue-attribute's object value
        /// 
        /// Returns null if EnumValue attribute do not exist or its value is actually null
        /// </summary>
        /// <example>
        /// <code class="language-csharp hljs">
        /// enum EnumColor
        /// {
        ///     [EnumText("White")]
        ///     [EnumValue(1234)]
        ///     Black,
        ///     
        ///     Pink
        /// }
        ///
        /// var value = EnumColor.Black.GetEnumValue();
        /// //1234, an int
        /// 
        /// var value = EnumColor.Pink.GetEnumValue();
        /// //null
        /// </code>
        /// </example>
        public static object GetEnumValue(this Enum enumField)
        {
            var valueAttribute = GetAttribute<EnumValueAttribute>(enumField, SystemType.EnumValueAttributeType);

            if (valueAttribute != null)
                return valueAttribute.Value;

            return enumField;
        }

        static T GetAttribute<T>(object value, Type type) where T : Attribute
        {
            if (value == null) return default;

            var enumType = value.GetType();

            var members = enumType.GetMember(value.ToString());

            return GetFirstAttributeFromMembers<T>(members, type);
        }

        static T GetFirstAttributeFromMembers<T>(MemberInfo[] members, Type type) where T : Attribute
        {
            if (members?.Length > 0)
            {
                foreach (var member in members)
                {
                    var attributes = member.GetCustomAttributes(type, inherit: false);
                    if (attributes.IsNot()) continue;

                    return (T)attributes[0];
                }
            }
            return default;
        }
    }
}
