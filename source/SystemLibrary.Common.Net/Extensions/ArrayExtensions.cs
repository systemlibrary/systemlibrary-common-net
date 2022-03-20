using System;
using System.Linq;

namespace SystemLibrary.Common.Net.Extensions
{
    /// <summary>
    /// This class contains extension methods for Arrays
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Append one array with other arrays, returning a new array of elements
        /// </summary>
        /// <returns>Returns null if everything is null, else a new array of items</returns>
        public static T[] Add<T>(this T[] current, params T[][] additional)
        {
            return Add(current, null, additional);
        }

        /// <summary>
        /// Add one array with another array, returning a new array of elements
        /// 
        /// - Pass a predicate method to filter out values
        /// </summary>
        /// <returns>Returns null if everything is null, else a new array of items</returns>
        public static T[] Add<T>(this T[] current, Func<T, bool> predicate, params T[][] additional)
        {
            if (additional == null ||
                additional.Length == 0 ||
                (additional.Length == 1 && additional[0] == null)) return current;

            var sum = current?.Length ?? 0;

            foreach (var add in additional)
                sum += add?.Length ?? 0;

            var result = (T[])Activator.CreateInstance(typeof(T[]), sum);

            var i = 0;

            if (current != null)
            {
                foreach (var v in current)
                {
                    if (predicate == null || predicate(v))
                    {
                        result[i] = v;
                        i++;
                    }
                }
            }

            foreach (var add in additional)
            {
                if (add != null)
                {
                    foreach (var v in add)
                    {
                        if (predicate == null || predicate(v))
                        {
                            result[i] = v;
                            i++;
                        }
                    }
                }
            }

            if (i != sum)
            {
                return result.Take(i).ToArray();
            }

            return result;
        }
    }
}
