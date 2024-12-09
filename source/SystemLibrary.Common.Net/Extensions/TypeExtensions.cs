using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SystemLibrary.Common.Net.Extensions;

/// <summary>
/// This class contains extension methods for Type
/// 
/// <para>For instance: Inherits()</para>
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Check if 'thisType' inherits or implements 'type
    /// 
    /// <para>False if both types are the same</para>
    /// </summary>
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
    /// <returns>true or false</returns>
    public static bool Inherits(this Type thisType, Type type)
    {
        if (thisType == type) return false;

        return type.IsAssignableFrom(thisType);
    }

    /// <summary>
    /// Check if type is a list or array
    /// </summary>
    /// <remarks>
    /// Does not check on IList, nor Dictionary, just List or Array
    /// </remarks>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var array = new string[] { "" };
    /// 
    /// var result = array.GetType().IsListOrArray();
    /// //result is true
    /// </code>
    /// </example>
    /// <returns>true or false</returns>
    public static bool IsListOrArray(this Type type)
    {
        if (type.IsArray) return true;
        if (type.IsGenericType == false) return false;

        return type.GetGenericTypeDefinition() == SystemType.ListType;
    }

    /// <summary>
    /// Checks if type is a dictionary
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var dictionary = new Dictionary&lt;string, string&gt;();
    /// 
    /// var result = dictionary.IsDictionary();
    /// //result is true
    /// </code>
    /// </example>
    /// <returns>true or false</returns>
    public static bool IsDictionary(this Type type)
    {
        if (type.IsArray) return false;
        if (type.IsGenericType == false) return false;

        return type.GetGenericTypeDefinition() == SystemType.DictionaryType;
    }

    /// <summary>
    /// Returns the Name of the Type that makes 'most sense'
    /// 
    /// <para>For generics, such as a IList, List, Dictionary, it will return the Name of the first generic Type specified</para>
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
    /// <returns>The type name</returns>
    public static string GetTypeName(this Type type)
    {
        if (type.IsListOrArray())
            return type.GenericTypeArguments[0].Name;

        if (type.IsDictionary())
            return type.GenericTypeArguments[0].Name;

        var firstGenericType = type.GetFirstGenericType();
        if (firstGenericType != null)
        {
            return firstGenericType.Name;
        }
        return type.Name;
    }

    /// <summary>
    /// Returns the first generic type specified, or null
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class Car
    /// {
    /// }
    /// 
    /// var type = typeof(List&lt;Car&gt;);
    /// var genericType = type.GetFirstGenericType();
    /// // genericType is now 'typeof(Car)'
    /// </code>
    /// </example>
    public static Type GetFirstGenericType(this Type type)
    {
        if (type == null || !type.IsGenericType) return default;

        var interfaces = type.GetInterfaces();

        if (interfaces != null)
        {
            foreach (Type @interface in interfaces)
            {
                if (@interface.IsGenericType)
                {
                    if (@interface.GetGenericTypeDefinition() == typeof(ICollection<>))
                        return @interface.GetGenericArguments()[0];
                    else if (@interface.GetGenericTypeDefinition() == typeof(IList<>))
                        return @interface.GetGenericArguments()[0];
                }
            }
        }

        return type.GetGenericArguments()[0];
    }

    /// <summary>
    /// Check if type is a KeyValuePair generic
    /// </summary>
    /// <param name="type"></param>
    /// <returns>true or false</returns>
    public static bool IsKeyValuePair(this Type type)
    {
        if (type == null) return false;

        if (type.IsGenericType && type.GetGenericTypeDefinition() == SystemType.KeyValueType)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Set a static member on a Type, no matter if it is internal or private
    /// </summary>
    public static void SetStaticMember(this Type type, string memberName, object value)
    {
        if (type == null) return;

        var property = type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic)?
                .Where(x => x.Name == memberName)?
                .FirstOrDefault();
        if (property == null)
        {
            property = type.GetProperties(BindingFlags.Static | BindingFlags.Public)?
                .Where(x => x.Name == memberName)?
                .FirstOrDefault();
        }
        if (property != null)
        {
            property.SetValue(null, value);
        }

        else
        {
            var field = type.GetFields(BindingFlags.Static | BindingFlags.NonPublic)?
            .Where(x => x.Name == memberName)?
            .FirstOrDefault();

            if (field == null)
            {
                field = type.GetFields(BindingFlags.Static | BindingFlags.Public)?
                 .Where(x => x.Name == memberName)?
                 .FirstOrDefault();
            }

            if (field != null)
            {
                field.SetValue(null, value);
            }
            else
            {
                throw new Exception(memberName + " do not exist on " + type.Name + " as a static member");
            }
        }
    }

    /// <summary>
    /// Returns true if internal else false
    /// </summary>
    /// <example>
    /// <code>
    /// var t = typeof(Car);
    /// var isInternal = t.IsInternal();
    /// </code>
    /// </example>
    /// <returns>True or false</returns>
    public static bool IsInternal(this Type type)
    {
        return type.IsNotPublic && !type.IsNested;
    }

    /// <summary>
    /// Returns a default instantiated value for value types, and null for reference types
    /// </summary>
    public static object Default(this Type type)
    {
        if (type.IsEnum) return 0;
        if (type.IsInterface) return null;

        if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }
}