
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net60, targetCount: 3, warmupCount: 5, invocationCount: 500000, launchCount: 5)]
[MemoryDiagnoser]
[RPlotExporter]
public class DummyBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public object Normal()
    {
        var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567";
        var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567";

        return data == data2;
    }

    [Benchmark]
    public object NormalSpan()
    {
        var data = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567";
        var data2 = "WHATEVER NOW vg.no&hello=world&world=&hello=world&world=&hello=world&world=1234567".AsSpan();

        return data == data2;
    }
}

