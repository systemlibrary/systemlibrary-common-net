using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SystemLibrary.Common.Net.Extensions
{
    /// <summary>
    /// This class contains extension methods for IEnumerables
    /// 
    /// For instance: Has, DistinctBy, Is, ...
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Select items grouped by a property of your choice
        /// </summary>
        /// <returns>A new list filtered on the property of your choice</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// class Car 
        /// {
        ///     public string Name;
        /// }
        /// 
        /// var car1 = new Car { Name = "Vehicle" }
        /// var car2 = new Car { Name = "Vehicle" }
        /// 
        /// var list = new List&lt;Car&gt; { car1, car2 }
        /// 
        /// var list = list.DistinctBy(x => x.Name).ToList();
        /// //list contains now 1 car
        /// </code>
        /// </example>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> selector)
            where T : class
        {
            return items.GroupBy(selector).Select(grp => grp.FirstOrDefault());
        }

        /// <summary>
        /// Check if the enumerable contains 'value'
        /// 
        /// Does not throw exception if enumerable is null nor if value is null
        /// 
        /// Uses Linq.Contains() internally
        /// </summary>
        /// <returns>Returns true or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        ///  var texts = new string[] { "Hello", "World" };
        ///  var has = texts.Has("Abc");
        ///  //has is False
        /// </code>
        /// </example>
        public static bool Has<T>(this IEnumerable<T> enumerable, T value) where T : IComparable, IConvertible
        {
            if (enumerable == null) return false;

            if (value == null) return false;

            return enumerable.Contains(value);
        }

        /// <summary>
        /// Check if the enumerable contains 'value'
        /// 
        /// Does not throw exception if enumerable is null nor if value is null
        /// 
        /// Uses Linq.Contains() internally
        /// </summary>
        /// <returns>Returns true or false</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var users = new List&lt;User&gt;();
        /// var user = new User();
        /// 
        /// users.Add(user);
        /// 
        /// var result = users.Has(user);
        /// //result is true, the list contains that specific user object
        /// </code>
        /// </example>
        public static bool Has<T>(this IEnumerable<T> enumerable, object value) where T : class
        {
            if (enumerable == null) return false;

            if (value == null) return false;

            if(value is T t)
                return enumerable.Contains(t);

            return false;
        }

        /// <summary>
        /// Check if the enumerable does not exist, if so it returns true, else if it is not null and it has at least 1 item, this will return false
        /// Note: the Linq.Any() throws exception if null while this function does not throw exception
        /// </summary>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var list = new List&lt;string&gt;();
        /// 
        /// var result = list.IsNot();
        /// 
        /// //result is true as the list contains 0 items
        /// 
        /// List&lt;string&gt; list = null;
        /// var result = list.IsNot();
        /// //result is true as the list currently is null
        /// </code>
        /// </example>
        public static bool IsNot<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return true;

            if (enumerable is ICollection iCollection)
            {
                if (iCollection is IList list)
                    return list.Count == 0 || (list.Count == 1 && list[0] == null);
                
                return iCollection.Count == 0;
            }

            return !enumerable.Any();
        }

        /// <summary>
        /// Check if the enumerable exists, if so it returns true, else if it is null or it has 0 items in it, this returns false
        /// Note: the Linq.Any() throws exception if null while this function does not throw exception
        /// </summary>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var list = new List&lt;string&gt;();
        /// list.Add("hello world");
        /// 
        /// var result = list.Is();
        /// 
        /// //result is true as the list contains 1 item
        /// 
        /// List&lt;string&gt; list = null;
        /// var result = list.Is();
        /// //result is false as the list is null
        /// </code>
        /// </example>
        public static bool Is<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return false;

            if (enumerable is ICollection iCollection)
                return iCollection.Count > 0;

            return enumerable.Any();
        }
    }
}