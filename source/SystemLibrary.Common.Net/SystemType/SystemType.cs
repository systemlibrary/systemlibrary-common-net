using System;
using System.Collections.Generic;

using SystemLibrary.Common.Net.Attributes;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// System type contains a list of static type variables so the call to 'typeof' is done once
    /// </summary>
    public static class SystemType
    {
        public static Type StringType = typeof(string);
        public static Type IntType = typeof(int);
        public static Type DateTimeType = typeof(DateTime);
        public static Type BoolType = typeof(bool);

        public static Type ListType = typeof(List<>);
        public static Type DictionaryType = typeof(Dictionary<,>);

        public static Type ObjectType = typeof(object);

        public static Type EnumValueAttributeType = typeof(EnumValueAttribute);
        public static Type EnumTextAttributeType = typeof(EnumTextAttribute);
    }
}
