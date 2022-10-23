
using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

using SystemLibrary.Common.Net.Extensions;
using SystemLibrary.Common.Net.Global;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net60, targetCount: 2, warmupCount: 2, invocationCount: 10000, launchCount: 3)]
[MemoryDiagnoser]
[RPlotExporter]
public class DummyBenchmarks
{
    static int UseStringInLoop()
    {
        var files = new string[] { "Hello", "World", "abc", "defgh.ko.klemewkgmewkgmgewegmgewegwgewoigewgewege" };
        var configurationName = "gmgewegwgewoigewgewege";
        int result = 0;
        foreach (var file in files)
        {
            if (file.Is())
            {
                var lowered = file.ToLower();

                if (lowered.Contains(configurationName))
                {
                    var values = lowered.Split('.');

                    if (values != null && values.Length > 1 && values[^2].Contains(configurationName))
                    {
                        if (!lowered.Contains("." + configurationName + "."))
                        {
                            result++;
                        }
                        else
                        {
                            result--;
                        }
                    }
                }
            }
        }

        return result;
    }

    static int UseSpanInLoop()
    {
        var files = new string[] { "Hello", "World", "abc", "defgh.ko.klemewkgmewkgmgewegmgewegwgewoigewgewege" };
        var configurationName = "gmgewegwgewoigewgewege";
        int result = 0;
        foreach (var file in files)
        {
            if (file.Is())
            {
                var span = file.ToLower().AsSpan();

                if (span.Contains(configurationName, StringComparison.Ordinal))
                {
                    var values = file.Split('.');

                    if (values != null && values.Length > 1 && values[^2].Contains(configurationName))
                    {
                        if (!span.Contains("." + configurationName + ".", StringComparison.Ordinal))
                        {
                            result++;
                        }
                        else
                        {
                            result--;
                        }
                    }
                }
            }
        }

        return result;
    }

    [GlobalSetup]
    public void Setup()
    {
        var result = UseStringInLoop();
        result += UseSpanInLoop();

        var r2 = "hello".EndsWithAnyCharacter("hahhhahahahheehahhae");

        if (r2)
        {
            result += 10;
        }
        if (result == int.MinValue)
            Dump.Write("Never occurs");

    }

    //[Benchmark]
    public string UseStringInLoopTest()
    {
        return UseStringInLoop().ToString();
    }

    //[Benchmark]
    //public string UseSpanInLoopTest()
    //{
    //    return UseSpanInLoop().ToString();
    //}

    //[Benchmark]
    //public object Normal()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567";
    //    return data.ContainsAny("AAAA", "bbbb", "cccc", "ddddd", "eee.com", "ffff", "1234567");
    //}

    //[Benchmark]
    //public object NormalSpan()
    //{
    //    var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567";
    //    return data.ContainsAnySpan("AAAA", "bbbb", "cccc", "ddddd", "eee.com", "ffff", "1234567");
    //}
}

