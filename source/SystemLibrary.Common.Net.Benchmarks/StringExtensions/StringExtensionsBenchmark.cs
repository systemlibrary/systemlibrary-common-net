using BenchmarkDotNet.Attributes;

using BenchmarkDotNet.Jobs;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[RPlotExporter]
[Config(typeof(BenchmarkConfig))]
public class StringExtensionsBenchmark
{
    string data;

    [GlobalSetup]
    public void Setup()
    {
        data = "Hello world";

        data.Obfuscate();
        data.Encrypt();
        data.ToBase64();
    }

    [Benchmark]
    public string Obfuscate()
    {
        return data.Obfuscate();
    }
    [Benchmark]
    public string Obfuscate2()
    {
        return data.Obfuscate2();
    }

    //[Benchmark]
    //public string Encrypt()
    //{
    //    return data.Encrypt("123");
    //}

    //[Benchmark]
    //public string Base64()
    //{
    //    return data.ToBase64();
    //}

    //[Benchmark]
    //public string Md5Hash()
    //{
    //    return data.ToMD5Hash();
    //}

    //[Benchmark]
    //public string Sha1Hash()
    //{
    //    return data.ToSha1Hash();
    //}
}
