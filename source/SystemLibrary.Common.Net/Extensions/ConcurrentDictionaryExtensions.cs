using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net.Extensions;

public static class ConcurrentDictionaryExtensions
{
    /// <summary>
    /// Try Get the item from the dictionary
    /// 
    /// If item is not in dictionary, the method getItem is invoked and result is added to dictionary and returned
    /// 
    /// NOTE: This is a static cache, objects lives as long as application runs in a thread-safe manner
    /// 
    /// NOTE 2: Item limit per dictionary is 100K before it is emptied and started over again
    /// </summary>
    public static T TryGet<T>(this ConcurrentDictionary<int, T> dictionary, int key, Func<T> getItem)
    {
        if (dictionary == null)
        {
            return getItem();
        }

        var hashCode = key.GetHashCode();

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
    /// Try Get the item from the dictionary
    /// 
    /// If item is not in dictionary, the method getItem is invoked and result is added to dictionary and returned
    /// 
    /// NOTE: This is a static cache, objects lives as long as application runs in a thread-safe manner
    /// 
    /// NOTE 2: Item limit per dictionary is 100K before it is emptied and started over again
    /// </summary>
    public static T TryGet<T>(this ConcurrentDictionary<int, T> dictionary, Type type, Func<T> getItem)
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
    /// Try Get the item from the dictionary
    /// 
    /// If item is not in dictionary, the method getItem is invoked and result is added to dictionary and returned
    /// 
    /// NOTE: This is a static cache, objects lives as long as application runs in a thread-safe manner
    /// 
    /// NOTE 2: Item limit per dictionary is 100K before it is emptied and started over again
    /// </summary>
    public static T TryGet<T>(this ConcurrentDictionary<string, T> dictionary, string key, Func<T> getItem)
    {
        if (dictionary == null)
            return getItem();

        if (key == null) 
            return getItem();

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
