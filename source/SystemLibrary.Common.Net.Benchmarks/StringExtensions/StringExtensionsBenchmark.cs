using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net70, warmupCount: 3, launchCount: 3, iterationCount: 4, invocationCount: 75)]
[MemoryDiagnoser]
[RPlotExporter]
public class StringExtensionsBenchmarks
{
    string bytes11;
    string bytes275;          //275 bytes
    string kiloBytes110;      //110KB   
    string kiloBytes550;      //550KB

    [GlobalSetup]
    public void Setup()
    {
        var res = "";

        bytes11 = "Hello world";

        try
        {
            for (int i = 0; i < 25; i++)
                bytes275 += bytes11;

            for (int i = 0; i < 400; i++)
                kiloBytes110 += bytes275;

            for (int i = 0; i < 5; i++)
                kiloBytes550 += kiloBytes110;

            res += bytes11.Obfuscate();
            res += bytes11.ToBase64();
            res += bytes11.ToMD5Hash();
            res += bytes11.ToSha1Hash();

            res += bytes275.Obfuscate();
            res += bytes275.ToBase64();
            res += bytes275.ToMD5Hash();
            res += bytes275.ToSha1Hash();

            res += kiloBytes110.Obfuscate();
            res += kiloBytes110.ToBase64();
            res += kiloBytes110.ToMD5Hash();
            res += kiloBytes110.ToSha1Hash();

            if (res.Length == 0)
                Dump.Write("Never occurs - just so the setup is being ran and not ignored");
        }
        catch (Exception ex)
        {
            Dump.Write(ex);
        }
    }

    [Benchmark]
    public string Base64_Bytes11()
    {
        return bytes11.ToBase64();
    }

    [Benchmark]
    public string Base64_Bytes275()
    {
        return bytes275.ToBase64();
    }

    [Benchmark]
    public string Base64_KiloBytes110()
    {
        return kiloBytes110.ToBase64();
    }

    [Benchmark]
    public string Base64_KiloBytes550()
    {
        return kiloBytes550.ToBase64();
    }

    [Benchmark]
    public string Obfuscate_Bytes11()
    {
        return bytes11.Obfuscate();
    }

    [Benchmark]
    public string ObfuscateBytes275()
    {
        return bytes275.Obfuscate();
    }

    [Benchmark]
    public string Obfuscate_KiloBytes110()
    {
        return kiloBytes110.Obfuscate();
    }

    [Benchmark]
    public string Obfuscate_KiloBytes550()
    {
        return kiloBytes550.Obfuscate();
    }

    [Benchmark]
    public string Md5Hash_Bytes11()
    {
        return bytes11.ToMD5Hash();
    }

    [Benchmark]
    public string Md5Hash_Bytes275()
    {
        return bytes275.ToMD5Hash();
    }

    [Benchmark]
    public string Md5Hash_KiloBytes110()
    {
        return kiloBytes110.ToMD5Hash();
    }

    [Benchmark]
    public string Md5Hash_KiloBytes550()
    {
        return kiloBytes550.ToMD5Hash();
    }

    [Benchmark]
    public string Sha1Hash_Bytes11()
    {
        return bytes11.ToSha1Hash();
    }

    [Benchmark]
    public string Sha1Hash_Bytes275()
    {
        return bytes275.ToSha1Hash();
    }

    [Benchmark]
    public string Sha1Hash_KiloBytes110()
    {
        return kiloBytes110.ToSha1Hash();
    }

    [Benchmark]
    public string Sha1Hash_KiloBytes550()
    {
        return kiloBytes550.ToSha1Hash();
    }

    [Benchmark]
    public string Sha256Hash_KiloBytes110()
    {
        return kiloBytes110.ToSha256Hash();
    }


    [Benchmark]
    public string Sha256Hash_KiloBytes550()
    {
        return kiloBytes550.ToSha256Hash();
    }

}
