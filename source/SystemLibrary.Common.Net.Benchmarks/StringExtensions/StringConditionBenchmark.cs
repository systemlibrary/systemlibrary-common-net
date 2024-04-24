using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net70, warmupCount: 2, invocationCount: 75000, launchCount: 4)]
[MemoryDiagnoser]
[RPlotExporter]
public class StringConditionBenchmarks
{
    static Type T;

    [GlobalSetup]
    public void Setup()
    {
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

