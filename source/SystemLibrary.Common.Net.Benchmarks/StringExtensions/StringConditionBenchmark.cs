using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net70, warmupCount: 1, invocationCount: 500, launchCount: 2)]
[MemoryDiagnoser]
[RPlotExporter]
public class StringConditionBenchmarks
{
    static Type T;
    string cacheKey;
    int MaxCacheContainers;
    Car c;

    [GlobalSetup]
    public void Setup()
    {
        MaxCacheContainers = 2;
        cacheKey = "hewkohwewoephjw 2u9234224ffAGHHRHEWHWascvwsfqw0 32039 3223u8r2 >| 4. |.-¤§.4|_51 12j qdoiqwj dwq";
        T = typeof(BenchMarkEnum);
        c = new Car();
        c.Name = "Hello";
        c.FirstName = "Hello world";
        c.LastName = "Hello Hello world long textHello world long textHello world long textHello world long textHello world long textHello world long textHello world long textworld long texHello world long textt";
        c.Age = 100;
        c.Age2 = 3333;
        c.Age3 = 2;
        c.Age4 = 1000000;
        c.dt = DateTime.Now;
        c.dt2 = DateTime.Now.AddDays(1);
    }

    [Benchmark]
    public bool FindTypes()
    {
        var types = Assemblies.FindAllTypesInheriting<Attribute>();

        return types.Any();
    }

    //[Benchmark]
    //public bool EndsWithAny()
    //{
    //    return "Hello world".EndsWithAny("aaaa", "bbbb", "cccc", "dddd", "eeee", "zzzzzz");
    //}

    //[Benchmark]
    //public bool EndsWithAny_Ordinal()
    //{
    //    return "Hello world".EndsWithAny(StringComparison.Ordinal, "aaaa", "bbbb", "cccc", "dddd", "eeee", "zzzzzz");
    //}

    //[Benchmark]
    //public bool EndsWith_Span()
    //{
    //    var data = "Hello world 123456";

    //    var r1 = data.StartsWithAny("world", "1234", "abc", "ello", "Hello");

    //    var r2 = data.StartsWithAny(null);

    //    var r3 = data.StartsWithAny("");

    //    var r4 = data.StartsWithAny("H");

    //    return r1 || r2 || r3 || r4;
    //}

    //[Benchmark]
    //public bool EndsWithAny_Multiple()
    //{
    //    return "Hello world".EndsWith("aaaa", StringComparison.Ordinal) ||
    //        "Hello world".EndsWith("bbbb", StringComparison.Ordinal) ||
    //        "Hello world".EndsWith("cccc", StringComparison.Ordinal) ||
    //        "Hello world".EndsWith("dddd", StringComparison.Ordinal) ||
    //        "Hello world".EndsWith("eeee", StringComparison.Ordinal) ||
    //        "Hello world".EndsWith("zzzzzz", StringComparison.Ordinal);
    //}

    //[Benchmark]
    //public int Bitshift()
    //{
    //    var cacheIndex = cacheKey.GetHashCode() & (MaxCacheContainers - 1);

    //    return cacheIndex;
    //}

    //[Benchmark]
    //public int MathAbs()
    //{
    //    var cacheIndex = Math.Abs(cacheKey.GetHashCode() % 4);

    //    return cacheIndex;
    //}

    //[Benchmark]
    //public string To_Server_Mapped_Path()
    //{
    //    var text = "https://www.sub.sub.subdomain.com/hello1/world2/?hello=world&hello=/world/";
    //    return text.ToPhysicalPath();
    //}

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

public class Car
{
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public int Age2 { get; set; }
    public int Age3 { get; set; }
    public int Age4 { get; set; }
    public DateTime dt { get; set; }
    public DateTime dt2 { get; set; }
}