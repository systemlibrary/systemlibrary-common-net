using System.Text;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SystemLibrary.Common.Net.Benchmarks.StringExtensions;

[SimpleJob(RuntimeMoniker.Net70, warmupCount: 2, launchCount: 2, iterationCount: 3, invocationCount: 100)]
[MemoryDiagnoser]
[RPlotExporter]
public class SpanVsSubstringBenchmark
{
    const string SampleText = "Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!lo world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!lo world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!lo world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!lo world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!Hello world 1234567890!!";

    [Benchmark]
    public byte[] Span_To_Bytes()
    {
        var span = SampleText.AsSpan(75, 275);

        Span<byte> bytes = new byte[span.Length];

        Encoding.UTF8.GetBytes(span, bytes);

        var a = bytes.ToArray();

        // Dump.Write(a[0] + " " + a[1] + " " + a[2] + " : " + a.Length);

        return a;
    }


    [Benchmark]
    public byte[] Span_Through_Array_To_Bytes()
    {
        var span = SampleText.AsSpan(75, 275);

        var a = Encoding.UTF8.GetBytes(span.ToArray());

        //Dump.Write(a[0] + " " + a[1] + " " + a[2] + " : " + a.Length);

        return a;
    }

    [Benchmark]
    public byte[] Substring_ToBytes()
    {
        var sub = SampleText.Substring(75, 275);

        var a = Encoding.UTF8.GetBytes(sub);

        //Dump.Write(a[0] + " " + a[1] + " " + a[2] + " : " + a.Length);

        return a;
    }

}
