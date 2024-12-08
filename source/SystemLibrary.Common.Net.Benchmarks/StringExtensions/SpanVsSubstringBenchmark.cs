using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net80, warmupCount: 2, launchCount: 3, iterationCount: 3, invocationCount: 330)]
[MemoryDiagnoser]
[RPlotExporter]
public class SpanVsSubstringBenchmark
{
    const string SampleText = "Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!lo world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!lo world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!lo world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!lo world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!";
    const string s = "hell";
    const string m = "Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890 world 1234567890!!Hello world 1234567890!!Hello world 1234567890!";
    [Benchmark]
    public string Enc()
    {
        return 0.ToString().Encrypt();
    }

    [Benchmark]
    public string CompressObfusBase64()
    {
        return 0.ToString().Compress().Obfuscate().ToBase64();
    }

    [Benchmark]
    public string ObfuscBass64Only()
    {
        return 0.ToString().Obfuscate().ToBase64();
    }


    //[Benchmark]
    //public int SmallHashCode()
    //{
    //    return s.GetHashCode();
    //}

    //[Benchmark]
    //public string SmallVSHashCode()
    //{
    //    return s.Length + "" + s[0] + "" + s[1] + s[s.Length - 1];
    //}

    //[Benchmark]
    //public int MediumHashCode()
    //{
    //    return m.GetHashCode();
    //}

    //[Benchmark]
    //public string MediumVSHashCode()
    //{
    //    return m.Length + "" + m[0] + "" + m[1] + m[m.Length - 1];
    //}

    //[Benchmark]
    //public int LargeHashCode()
    //{
    //    return SampleText.GetHashCode();
    //}

    //[Benchmark]
    //public string LargeVSHashCode()
    //{
    //    return SampleText.Length + "" + SampleText[0] + "" + SampleText[1] + SampleText[SampleText.Length - 1];
    //}


    //[Benchmark]
    //public byte[] Span_To_Bytes()
    //{
    //    var span = SampleText.AsSpan(75, 275);

    //    Span<byte> bytes = new byte[span.Length];

    //    Encoding.UTF8.GetBytes(span, bytes);

    //    var a = bytes.ToArray();

    //    // Dump.Write(a[0] + " " + a[1] + " " + a[2] + " : " + a.Length);

    //    return a;
    //}


    //[Benchmark]
    //public byte[] Span_Through_Array_To_Bytes()
    //{
    //    var span = SampleText.AsSpan(75, 275);

    //    var a = Encoding.UTF8.GetBytes(span.ToArray());

    //    //Dump.Write(a[0] + " " + a[1] + " " + a[2] + " : " + a.Length);

    //    return a;
    //}

    //[Benchmark]
    //public byte[] Substring_ToBytes()
    //{
    //    var sub = SampleText.Substring(75, 275);

    //    var a = Encoding.UTF8.GetBytes(sub);

    //    //Dump.Write(a[0] + " " + a[1] + " " + a[2] + " : " + a.Length);

    //    return a;
    //}

}
