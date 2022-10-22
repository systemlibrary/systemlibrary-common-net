using System;

namespace SystemLibrary.Common.Net.Extensions;

/// <summary>
/// This class contains extension methods for Type
/// 
/// For instance: Inherits()
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Check if 'thisType' inherits (implements) 'type'
    ///
    /// Returns false if both types are the same or if 'type' is not inherited by 'thisType'
    /// 
    /// For interfaces this can be read as 'implements'
    /// </summary>
    /// <returns>Returns true or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class Car : IVehicle 
    /// {
    /// }
    /// 
    /// var result = typeof(Car).Inherits(typeof(IVehicle));
    /// // result is true, as it inherits/implements IVehicle
    /// 
    /// var result = typeof(Car).Inherits(typeof(Car));
    /// // result is false, as Car cannot inherit/implement itself
    /// </code>
    /// </example>
    public static bool Inherits(this Type thisType, Type type)
    {
        if (thisType == type) return false;

        return type.IsAssignableFrom(thisType);
    }

    /// <summary>
    /// Check if type is a list or array
    /// </summary>
    /// <returns>Returns true or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var array = new string[] { "" };
    /// 
    /// var result = array.GetType().IsListOrArray();
    /// //result is true
    /// </code>
    /// </example>
    public static bool IsListOrArray(this Type type)
    {
        if (type.IsArray) return true;
        if (type.IsGenericType == false) return false;

        return type.GetGenericTypeDefinition() == SystemType.ListType;
    }

    /// <summary>
    /// Checks if type is a dictionary
    /// </summary>
    /// <returns>Returns true or false</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var dictionary = new Dictionary&lt;string, string&gt;();
    /// 
    /// var result = dictionary.IsDictionary();
    /// //result is true
    /// </code>
    /// </example>
    public static bool IsDictionary(this Type type)
    {
        if (type.IsArray) return false;
        if (type.IsGenericType == false) return false;

        return type.GetGenericTypeDefinition() == SystemType.DictionaryType;
    }

    /// <summary>
    /// Returns the type.Name
    /// 
    /// For generics such as List, Dictionary or Array, it returns the "inner type name" of those
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class Car
    /// {
    /// }
    /// 
    /// var result = typeof(Car).GetTypeName();
    /// // result is "Car"
    /// 
    /// var list = new List&lt;Car&gt;
    /// var result = list.GetType().GetTypeName();
    /// // result is "Car"
    /// 
    /// var result = typeof(List&lt;Car&gt;).GetTypeName();
    /// // result is "Car"
    /// 
    /// var result = typeof(Car[]).GetTypeName();
    /// // result is "Car"
    /// </code>
    /// </example>
    public static string GetTypeName(this Type type)
    {
        if (type.IsListOrArray())
            return type.GenericTypeArguments[0].Name;

        if (type.IsDictionary())
            return type.GenericTypeArguments[0].Name;

        return type.Name;
    }
}
