using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net;

public static class DictionaryCache
{
    internal static ConcurrentDictionary<int, MemberInfo[]> EnumMemberInfoCache;
    internal static ConcurrentDictionary<int, PropertyInfo[]> MergeTypePropertiesCache;

    static DictionaryCache()
    {
        EnumMemberInfoCache = new ConcurrentDictionary<int, MemberInfo[]>();
        MergeTypePropertiesCache = new ConcurrentDictionary<int, PropertyInfo[]>();
    }

    /// <summary>
    /// Return the cache variation of GetItem() based on the Type where the dictionary key is int (hashcode of Type), or if not exist add the result of getItem to cache and then return it
    /// </summary>
    public static T Get<T>(Type type, ConcurrentDictionary<int, T> dictionary, Func<T> getItem)
    {
        if (dictionary == null)
        {
            return getItem();
        }

        var hashCode = type.GetHashCode();

        if (!dictionary.TryGetValue(hashCode, out var result))
        {
            if (dictionary.Count > 100000)
            {
                dictionary.Clear();
            }
            result = getItem();

            dictionary.TryAdd(hashCode, result);
        }

        return result;
    }

    /// <summary>
    /// Return the cache variation of GetItem() based on the string, or if not exist add the result of getItem to cache and then return it
    /// </summary>
    public static T Get<T>(string key, ConcurrentDictionary<string, T> dictionary, Func<T> getItem)
    {
        if (dictionary == null)
        {
            return getItem();
        }

        if (!dictionary.TryGetValue(key, out var result))
        {
            if (dictionary.Count > 100000)
            {
                dictionary.Clear();
            }
            result = getItem();

            dictionary.TryAdd(key, result);
        }

        return result;
    }
}