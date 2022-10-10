
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

//[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net60, warmupCount: 3, launchCount: 2, targetCount: 4, invocationCount: 50)]
[MemoryDiagnoser]
[RPlotExporter]
public class StringExtensionsBenchmark
{
    string data;
    string dataLong;

    [GlobalSetup]
    public void Setup()
    {
        var res = "";
        data = "Hello world";

        try
        {
            dataLong = data + data + data + data + data + data + data;
            dataLong = dataLong + dataLong + dataLong + dataLong + dataLong + dataLong;
            dataLong = dataLong + dataLong + dataLong + dataLong + dataLong + dataLong;
            dataLong = dataLong + dataLong + dataLong + dataLong + dataLong + dataLong;
            dataLong = dataLong + dataLong + dataLong + dataLong + dataLong + dataLong;
            dataLong = dataLong + dataLong + dataLong + dataLong + dataLong + dataLong + dataLong + dataLong + dataLong;
            res += data.Obfuscate();
            res += data.ToBase64();
            res += data.ToMD5Hash();
            res += data.ToSha1Hash();
        }
        catch(Exception ex)
        {
            Dump.Write(ex);
        }
    }

    [Benchmark]
    public string Obfuscate()
    {
        return data.Obfuscate();
    }

    [Benchmark]
    public string Base64()
    {
        return data.ToBase64();
    }

    [Benchmark]
    public string Base64Long()
    {
        //Can use this method if the filesize is less than 100kb / less than 100.000 chars roughly
        //Else Md5Hash is starting to get a lot faster
        //Memory print of Md5Hash is also a lot smaller
        return (dataLong.Length + dataLong).ToBase64();
    }

    [Benchmark]
    public string Md5HashLong()
    {
        return dataLong.ToMD5Hash();
    }

    [Benchmark]
    public string Md5Hash()
    {
        return data.ToMD5Hash();
    }

    [Benchmark]
    public string Sha1Hash()
    {
        return data.ToSha1Hash();
    }
}
