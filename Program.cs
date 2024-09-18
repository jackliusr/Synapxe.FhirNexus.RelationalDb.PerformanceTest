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

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.4894/22H2/2022Update)

SearchByIdentifierBenchmark
| Method                 | Mean     | Error    | StdDev   | Median   |
|----------------------- |---------:|---------:|---------:|---------:|
| FullTablesDbContext    | 24.26 ms | 1.473 ms | 4.031 ms | 23.10 ms |
| ReducedTablesDbContext | 21.59 ms | 1.006 ms | 2.788 ms | 20.65 ms |
| JsonColumnDbContext    | 13.16 ms | 0.347 ms | 0.991 ms | 12.98 ms |

BulkReadBenchmark.
| Method                  | Mean     | Error    | StdDev   | Median   |
|------------------------ |---------:|---------:|---------:|---------:|
| FullTablesDbContext2    | 25.20 ms | 1.704 ms | 4.996 ms | 23.51 ms |
| ReducedTablesDbContext2 | 19.69 ms | 1.883 ms | 5.522 ms | 17.06 ms |
| JsonColumnDbContext2    | 10.02 ms | 0.198 ms | 0.372 ms | 10.09 ms |

SearchByNameBenchmark
| Method                 | Mean     | Error     | StdDev    | Median   |
|----------------------- |---------:|----------:|----------:|---------:|
| FullTablesDbContext    | 2.979 ms | 0.0824 ms | 0.2298 ms | 2.986 ms |
| ReducedTablesDbContext | 3.061 ms | 0.1759 ms | 0.5131 ms | 2.915 ms |
| JsonColumnDbContext    | 3.630 ms | 0.1005 ms | 0.2850 ms | 3.593 ms |
 */

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
