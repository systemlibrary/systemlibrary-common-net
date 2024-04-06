using BenchmarkDotNet.Running;

using SystemLibrary.Common.Net.Benchmarks.StringExtensions;


//BenchmarkRunner.Run<StringCondition>();
//BenchmarkRunner.Run<StringExtensionsBenchmarks>();
BenchmarkRunner.Run<SpanVsSubstringBenchmark>();

