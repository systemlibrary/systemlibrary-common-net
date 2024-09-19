using BenchmarkDotNet.Running;

using SystemLibrary.Common.Net.Benchmarks.StringExtensions;


BenchmarkRunner.Run<StringConditionBenchmarks>();
//BenchmarkRunner.Run<StringExtensionsBenchmarks>();
//BenchmarkRunner.Run<SpanVsSubstringBenchmark>();

