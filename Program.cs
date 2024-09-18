using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Synapxe.FhirNexus.RelationalDb.PerformanceTest;
/*
 ManualConfig
                    .Create(DefaultConfig.Instance)
                    .WithOptions(ConfigOptions.JoinSummary)
                    .WithOptions(ConfigOptions.DisableLogFile)
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
 */


BenchmarkRunner.Run<SearchByIdentifierBenchmark>(ManualConfig
                    .Create(DefaultConfig.Instance)
                    .WithOptions(ConfigOptions.JoinSummary)
                    .WithOptions(ConfigOptions.DisableLogFile)
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                    .WithBuildTimeout(TimeSpan.FromMinutes(5)));
BenchmarkRunner.Run<BulkReadBenchmark>(ManualConfig
                    .Create(DefaultConfig.Instance)
                    .WithOptions(ConfigOptions.JoinSummary)
                    .WithOptions(ConfigOptions.DisableLogFile)
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                    .WithBuildTimeout(TimeSpan.FromMinutes(5)));
BenchmarkRunner.Run<SearchByNameBenchmark>(ManualConfig
                    .Create(DefaultConfig.Instance)
                    .WithOptions(ConfigOptions.JoinSummary)
                    .WithOptions(ConfigOptions.DisableLogFile)
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                    .WithBuildTimeout(TimeSpan.FromMinutes(5)));


/*
 * 
 *   BenchmarkRunner
            .Run<Benchmarks>(
                ManualConfig
                    .Create(DefaultConfig.Instance)
                    .WithOptions(ConfigOptions.JoinSummary)
                    .WithOptions(ConfigOptions.DisableLogFile)
                    // or
                    .WithOptions(ConfigOptions.JoinSummary | ConfigOptions.DisableLogFile));
 */

/* Postgresql

SearchByIdentifierBenchmark
| Method                   | Mean     | Error    | StdDev    |
|------------------------- |---------:|---------:|----------:|
| FullTablesDbContext      | 55.18 ms | 2.863 ms |  8.442 ms |
| ReducedTablesDbContext   | 56.29 ms | 4.574 ms | 13.485 ms |
| JsonColumnDbContext      | 79.76 ms | 4.966 ms | 14.249 ms |
| FullTablesDbContextPg    | 48.31 ms | 1.561 ms |  4.479 ms |
| ReducedTablesDbContextPg | 32.92 ms | 1.411 ms |  4.117 ms |
| JsonColumnDbContextPg    | 28.36 ms | 0.529 ms |  0.954 ms |


BulkReadBenchmark
| Method                    | Mean     | Error    | StdDev   | Median   |
|-------------------------- |---------:|---------:|---------:|---------:|
| FullTablesDbContext2      | 88.80 ms | 3.469 ms | 9.841 ms | 89.45 ms |
| ReducedTablesDbContext2   | 24.42 ms | 0.577 ms | 1.664 ms | 24.01 ms |
| JsonColumnDbContext2      | 12.45 ms | 0.240 ms | 0.225 ms | 12.52 ms |
| FullTablesDbContext2Pg    | 48.69 ms | 0.972 ms | 2.420 ms | 48.01 ms |
| ReducedTablesDbContext2Pg | 24.07 ms | 0.478 ms | 0.671 ms | 23.88 ms |
| JsonColumnDbContext2Pg    | 14.59 ms | 0.350 ms | 0.975 ms | 14.31 ms |

SearchByNameBenchmark
| Method                   | Mean      | Error     | StdDev    | Median    |
|------------------------- |----------:|----------:|----------:|----------:|
| FullTablesDbContext      | 44.686 ms | 3.0452 ms | 8.5891 ms | 43.933 ms |
| ReducedTablesDbContext   | 39.312 ms | 2.8135 ms | 8.2516 ms | 40.984 ms |
| JsonColumnDbContext      | 36.268 ms | 2.5500 ms | 7.5189 ms | 38.350 ms |
| FullTablesDbContextPg    | 15.796 ms | 0.5458 ms | 1.5921 ms | 15.499 ms |
| ReducedTablesDbContextPg |  5.293 ms | 0.1289 ms | 0.3550 ms |  5.243 ms |
| JsonColumnDbContextPg    | 13.436 ms | 0.5183 ms | 1.5202 ms | 13.550 ms |


/*

SearchByIdentifierBenchmark result:
| Method                 | Mean     | Error    | StdDev   |
|----------------------- |---------:|---------:|---------:|
| FullTablesDbContext    | 25.95 ms | 0.518 ms | 1.241 ms |
| ReducedTablesDbContext | 23.37 ms | 0.465 ms | 1.040 ms |
| JsonColumnDbContext    | 45.87 ms | 0.913 ms | 1.823 ms | 

BulkReadBenchmark result:
| Method                  | Mean      | Error     | StdDev    |
|------------------------ |----------:|----------:|----------:|
| FullTablesDbContext2    | 48.646 ms | 0.9674 ms | 1.1516 ms |
| ReducedTablesDbContext2 | 10.062 ms | 0.2011 ms | 0.4108 ms |
| JsonColumnDbContext2    |  7.884 ms | 0.0852 ms | 0.0797 ms |

SearchByNameBenchmark - target 1 record
| Method                 | Mean     | Error    | StdDev   |
|----------------------- |---------:|---------:|---------:|
| FullTablesDbContext    | 18.60 ms | 0.370 ms | 0.827 ms |
| ReducedTablesDbContext | 15.72 ms | 0.314 ms | 0.497 ms |
| JsonColumnDbContext    | 20.61 ms | 0.411 ms | 0.792 ms |

SearchByNameBenchmark - target 6 record
| Method                 | Mean     | Error    | StdDev   |
|----------------------- |---------:|---------:|---------:|
| FullTablesDbContext    | 20.26 ms | 0.452 ms | 1.333 ms |
| ReducedTablesDbContext | 16.53 ms | 0.329 ms | 0.687 ms |
| JsonColumnDbContext    | 20.34 ms | 0.396 ms | 0.694 ms |

SearchByNameBenchmark - target 50 record
| Method                 | Mean     | Error    | StdDev   |
|----------------------- |---------:|---------:|---------:|
| FullTablesDbContext    | 32.88 ms | 0.673 ms | 1.761 ms |
| ReducedTablesDbContext | 21.02 ms | 0.407 ms | 0.775 ms |
| JsonColumnDbContext    | 23.16 ms | 0.460 ms | 1.101 ms |

SearchByNameBenchmark - target 1100 record
| Method                 | Mean      | Error    | StdDev    |
|----------------------- |----------:|---------:|----------:|
| FullTablesDbContext    | 401.56 ms | 7.976 ms | 14.177 ms |
| ReducedTablesDbContext | 141.14 ms | 2.344 ms |  2.078 ms |
| JsonColumnDbContext    |  91.38 ms | 1.789 ms |  1.757 ms |

 */

//var stopwatch = new System.Diagnostics.Stopwatch();
//stopwatch.Start();
//await Helper.PrepareDatabase();
//stopwatch.Stop();
//Console.WriteLine("Done");
//Console.WriteLine("Elapsed time: " + stopwatch.ElapsedMilliseconds + "ms");
