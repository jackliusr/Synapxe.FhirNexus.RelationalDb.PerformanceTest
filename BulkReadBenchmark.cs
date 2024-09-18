using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    public class BulkReadBenchmark
    {
        private FullTablesDbContext _fullTablesDbContext;
        private ReducedTablesDbContext _reducedTablesDbContext;
        private JsonColumnDbContext _jsonColumnDbContext;
        private FullTablesDbContextPg _fullTablesDbContextPg;
        private ReducedTablesDbContextPg _reducedTablesDbContextPg;
        private JsonColumnDbContextPg _jsonColumnDbContextPg;

        [GlobalSetup]
        public async void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<FullTablesDbContext>();
            services.AddDbContext<ReducedTablesDbContext>();
            services.AddDbContext<JsonColumnDbContext>();
            services.AddDbContext<FullTablesDbContextPg>();
            services.AddDbContext<ReducedTablesDbContextPg>();
            services.AddDbContext<JsonColumnDbContextPg>();
            services.AddSingleton(Helper.GetFhirRelationalOptions<FullTablesDbContext>());
            services.AddSingleton(Helper.GetFhirRelationalOptions<ReducedTablesDbContext>());
            services.AddSingleton(Helper.GetFhirRelationalOptions<FullTablesDbContextPg>());
            services.AddSingleton(Helper.GetFhirRelationalOptions<ReducedTablesDbContextPg>());
            var provider = services.BuildServiceProvider();
            _fullTablesDbContext = provider.GetRequiredService<FullTablesDbContext>();
            _reducedTablesDbContext = provider.GetRequiredService<ReducedTablesDbContext>();
            _jsonColumnDbContext = provider.GetRequiredService<JsonColumnDbContext>();

            _fullTablesDbContextPg = provider.GetRequiredService<FullTablesDbContextPg>();
            _reducedTablesDbContextPg = provider.GetRequiredService<ReducedTablesDbContextPg>();
            _jsonColumnDbContextPg = provider.GetRequiredService<JsonColumnDbContextPg>();

        }

        [Benchmark]
        public void FullTablesDbContext2()
        {
            _fullTablesDbContext.Organization.Take(100).ToList();
        }

        [Benchmark]
        public void ReducedTablesDbContext2()
        {
            _reducedTablesDbContext.Organization.Take(100).ToList();
        }

        [Benchmark]
        public void JsonColumnDbContext2()
        {
            _jsonColumnDbContext.Organization.Take(100).ToList();
        }

        [Benchmark]
        public void FullTablesDbContext2Pg()
        {
            _fullTablesDbContextPg.Organization.Take(100).ToList();
        }

        [Benchmark]
        public void ReducedTablesDbContext2Pg()
        {
            _reducedTablesDbContextPg.Organization.Take(100).ToList();
        }

        [Benchmark]
        public void JsonColumnDbContext2Pg()
        {
            _jsonColumnDbContextPg.Organization.Take(100).ToList();
        }
    }
}
