using BenchmarkDotNet.Running;
using Synapxe.FhirNexus.RelationalDb.PerformanceTest;

BenchmarkRunner.Run<ComparePerformance>();

/*
| Method                 | Mean     | Error    | StdDev   |
|----------------------- |---------:|---------:|---------:|
| FullTablesDbContext    | 25.95 ms | 0.518 ms | 1.241 ms |
| ReducedTablesDbContext | 23.37 ms | 0.465 ms | 1.040 ms |
| JsonColumnDbContext    | 45.87 ms | 0.913 ms | 1.823 ms | 
 */

//var stopwatch = new System.Diagnostics.Stopwatch();
//stopwatch.Start();
//await Helper.PrepareDatabase();
//stopwatch.Stop();
//Console.WriteLine("Done");
//Console.WriteLine("Elapsed time: " + stopwatch.ElapsedMilliseconds + "ms");
