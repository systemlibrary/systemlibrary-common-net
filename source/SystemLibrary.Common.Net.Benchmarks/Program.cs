using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using SystemLibrary.Common.Net.Benchmarks.StringExtensions;

BenchmarkRunner.Run<StringExtensionsBenchmark>();

class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        var job = new Job();

        AddJob(job);
    }
}