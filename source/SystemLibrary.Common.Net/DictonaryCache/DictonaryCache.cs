using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net.Cache;

internal static class DictionaryCache
{
    internal static ConcurrentDictionary<int, MemberInfo[]> EnumMemberInfoCache;
    internal static ConcurrentDictionary<int, PropertyInfo[]> MergeTypePropertiesCache;

    static DictionaryCache()
    {
        EnumMemberInfoCache = new ConcurrentDictionary<int, MemberInfo[]>();
        MergeTypePropertiesCache = new ConcurrentDictionary<int, PropertyInfo[]>();
    }
}
