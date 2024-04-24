using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net70, warmupCount: 2, invocationCount: 75000, launchCount: 4)]
[MemoryDiagnoser]
[RPlotExporter]
public class StringConditionBenchmarks
{
    static Type T;

    static ConcurrentDictionary<int, FieldInfo[]> Cached;

    [GlobalSetup]
    public void Setup()
    {
        Cached = new ConcurrentDictionary<int, FieldInfo[]>();
        T = typeof(BenchMarkEnum);
    }

    //[Benchmark]
    //public object String_Condition_Are_Equals()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567";
    //    var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567";

    //    return data == data2;
    //}

    //[Benchmark]
    //public object String_Condition_AsSpan_Are_Equals()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567".AsSpan();
    //    var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567".AsSpan();

    //    return data == data2;
    //}

    //[Benchmark]
    //public object String_Condition_Are_Equals_Short()
    //{
    //    var data = "WHATEVER NOW";
    //    var data2 = "WHATEVER NOW";

    //    return data == data2;
    //}


    //[Benchmark]
    //public object String_Condition_AsSpan_Are_Equals_Short()
    //{
    //    var data = "WHATEVER NOW".AsSpan();
    //    var data2 = "WHATEVER NOW".AsSpan();

    //    return data == data2;
    //}

    //[Benchmark]
    //public object String_Condition_Are_Not_Equal_Short()
    //{
    //    var data = "WHATEVER NOW12344";
    //    var data2 = "WHATEVER NOW12345";

    //    return data == data2;

    //}

    //[Benchmark]
    //public object String_Condition_AsSpan_Are_Not_Equal_Short()
    //{
    //    var data = "WHATEVER NOW12344".AsSpan();
    //    var data2 = "WHATEVER NOW12345".AsSpan();

    //    return data == data2;
    //}

    //[Benchmark]
    //public object String_Condition_Are_Not_Equal_Long()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234557";
    //    var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567";

    //    return data == data2;
    //}


    //[Benchmark]
    //public object String_Condition_AsSpan_Are_Not_Equal_Long()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234557".AsSpan();
    //    var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567".AsSpan();

    //    return data == data2;
    //}

    //[Benchmark]
    //public object String_Condition_Are_Not_Equal_In_Length_Long()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=123457";
    //    var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=12345";

    //    return data == data2;
    //}

    //[Benchmark]
    //public object String_Condition_AsSpan_Are_Not_Equal_In_Length_Long()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=123457".AsSpan();
    //    var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=12345".AsSpan();

    //    return data == data2;
    //}

    //[Benchmark]
    //public object String_Condition_AsSpan_Are_Not_Equal_In_Length_Long()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=123457".AsSpan();
    //    var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=12345".AsSpan();

    //    return data == data2;
    //}


    //[Benchmark]
    //public object BenchMarkEnumGetMembers()
    //{
    //    var type = typeof(BenchMarkEnum);

    //    var member = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

    //    return member;
    //}

    //[Benchmark]
    //public object BenchMarkEnumGetMembersAsCached()
    //{
    //    var type = typeof(BenchMarkEnum);

    //    var key = type.Namespace + type.Name + nameof(BenchmarkAttribute);

    //    if (MemberInfoCache.TryGetValue(key, out var value))
    //        return value;

    //    var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

    //    MemberInfoCache.TryAdd(key, members);

    //    return members;
    //}

    

    [Benchmark]
    public object A1()
    {
        return Func(() => "haha");
    }

    [Benchmark]
    public object A2()
    {
        return Func2(() => "haha");
    }

    [Benchmark]
    public object A3()
    {
        var a = 222;
        var b = "hello world";
        var c = DateTime.Now;
        return Func(() => a + b + c);
    }

    [Benchmark]
    public object A4()
    {
        var a = 222;
        var b = "hello world";
        var c = DateTime.Now;
        return Func2(() => a + b + c);
    }

    static StringBuilder Func<T>(Func<T> getItem)
    {
        var key = new StringBuilder("common.web.cache");
        var getItemMethod = getItem.Method;

        key.Append(getItemMethod.Name);
        key.Append(getItemMethod.DeclaringType?.FullName);
        key.Append(getItemMethod.ReturnType?.FullName);

        var target = getItem.Target;
        if (target != null)
        {
            var type = target.GetType();
            var fields = type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            if (fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    var value = field.GetValue(target);
                    if (value != null)
                        key.Append(value.ToString());
                }
            }
        }
        return key;
    }

    static StringBuilder Func2<T>(Func<T> getItem)
    {
        var key = new StringBuilder("common.web.cache", capacity:  255);

        var getItemMethod = getItem.Method;

        key.Append(getItemMethod.Name);
        key.Append(getItemMethod.DeclaringType.GetHashCode());
        key.Append(getItemMethod.ReturnType.GetHashCode());

        var target = getItem.Target;
        if (target != null)
        {
            var type = target.GetType();
            var fields = Cached.TryGet(type, () =>
            {
                return type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            });

            if (fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    key.Append(field.Name);

                    var value = field.GetValue(target);

                    if (value != null)
                    {
                        key.Append(value.GetHashCode());
                    }
                }
            }
        }
        return key;
    }
}

public enum BenchMarkEnum
{
    HelloWorld,
    AnotherWorld,
    BrightNewWorld,
    BrightNewWorld2,
    BrightNewWorld3,
    Another,
    Value,
    Secondary,
    _999,
    _10000
}

