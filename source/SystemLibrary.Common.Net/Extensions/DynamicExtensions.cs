using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

using SystemLibrary.Common.Net.Cache;

namespace SystemLibrary.Common.Net.Extensions;

/// <summary>
/// This class contains extension methods for Dynamic objects
/// 
/// WARNING: Current version of C# does not allow extension methods on 'dynamic', hence these are static methods
/// </summary>
public static class DynamicExtensions
{
    /// <summary>
    /// Merge dynamic objects into a one new dynamic object
    /// 
    /// The latter updating value takes precedence if both objects contains same property name
    /// </summary>
    /// <param name="source">An anonymous/dynamic object</param>
    /// <param name="updates">One or more anonymous/dynamic objects, but if error occurs, cast the 'updating' objects to an actual object before invoking this method</param>
    /// <returns>Returns a new dynamic object with the merge result, can be casted to IDictionary&lt;string, object&gt;</returns>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var a = new
    /// {
    ///     firstName = "world",
    ///     age = 1
    /// };
    /// var b = new
    /// {
    ///     firstName = "hello",
    ///     Age = 2
    /// };
    /// var c = new 
    /// {
    ///    Age = 10   
    /// }
    ///
    /// var d = DynamicExtensions.Merge(a, b, c);
    /// 
    /// // d contains 3 properties: firstName, age, Age
    /// // d's property values are now: hello, 1, 10
    /// // var dictionary = d as IDictionary&lt;string, object&gt;
    /// // d can be cast to dictionary
    /// // dictionary["Age"] will return 10
    /// </code>
    /// </example>
    public static dynamic Merge(dynamic source, params object[] updates)
    {
        if (source == null && updates.IsNot()) return null;

        var dictionary = new ExpandoObject() as IDictionary<string, object>;

        if (source != null)
        {
            var type = source.GetType();
            var hashCode = type.GetHashCode();
            if(!DictionaryCache.MergeTypePropertiesCache.TryGetValue(hashCode, out PropertyInfo[] properties))
            {
                properties = type.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance) as PropertyInfo[];

                DictionaryCache.MergeTypePropertiesCache.TryAdd(hashCode, properties);
            }

            foreach (PropertyInfo property in properties)
                if (property.CanRead)
                    dictionary[property.Name] = property.GetValue(source);
        }

        if (updates.Is())
        {
            foreach (var update in updates)
            {
                var type = update.GetType();
                var hashCode = type.GetHashCode();
                if (!DictionaryCache.MergeTypePropertiesCache.TryGetValue(hashCode, out PropertyInfo[] properties))
                {
                    properties = type.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

                    DictionaryCache.MergeTypePropertiesCache.TryAdd(hashCode, properties);
                }

                foreach (PropertyInfo property in properties)
                    if (property.CanRead)
                        dictionary[property.Name] = property.GetValue(update);
            }
        }

        return dictionary as ExpandoObject;
    }
}
