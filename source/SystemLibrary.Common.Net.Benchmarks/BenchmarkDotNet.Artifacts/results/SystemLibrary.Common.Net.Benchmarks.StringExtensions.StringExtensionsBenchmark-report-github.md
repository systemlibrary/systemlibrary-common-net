``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2006/21H2/November2021Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.304
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  Job-FFVYUQ : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  .NET 6.0   : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2

Runtime=.NET 6.0  

```
|     Method |        Job | MinInvokeCount | InvocationCount | IterationCount | MaxIterationCount | MaxWarmupIterationCount | MinIterationCount | MinWarmupIterationCount | UnrollFactor | WarmupCount |        Mean |    Error |   StdDev |   Gen0 | Allocated |
|----------- |----------- |--------------- |---------------- |--------------- |------------------ |------------------------ |------------------ |------------------------ |------------- |------------ |------------:|---------:|---------:|-------:|----------:|
|  Obfuscate | Job-FFVYUQ |              1 |               2 |              1 |                 2 |                       2 |                 1 |                       1 |            2 |           1 | 1,225.00 ns |       NA | 0.000 ns |      - |     360 B |
| Obfuscate2 | Job-FFVYUQ |              1 |               2 |              1 |                 2 |                       2 |                 1 |                       1 |            2 |           1 | 1,250.00 ns |       NA | 0.000 ns |      - |     456 B |
|  Obfuscate |   .NET 6.0 |        Default |         Default |        Default |           Default |                 Default |           Default |                 Default |           16 |     Default |    92.46 ns | 1.898 ns | 3.223 ns | 0.0191 |     120 B |
| Obfuscate2 |   .NET 6.0 |        Default |         Default |        Default |           Default |                 Default |           Default |                 Default |           16 |     Default |   137.83 ns | 1.918 ns | 1.700 ns | 0.0343 |     216 B |
