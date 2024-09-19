using System.Collections.Concurrent;
using System.Reflection;

namespace SystemLibrary.Common.Net;

internal static class Dictionaries
{
    internal static ConcurrentDictionary<int, MemberInfo[]> TypeEnumStaticMembers;
    internal static ConcurrentDictionary<int, PropertyInfo[]> MergeProperties;

    static Dictionaries()
    {
        TypeEnumStaticMembers = new ConcurrentDictionary<int, MemberInfo[]>();
        MergeProperties = new ConcurrentDictionary<int, PropertyInfo[]>();
    }
}