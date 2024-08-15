using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    public class BulkReadBenchmark
    {
        private FullTablesDbContext _fullTablesDbContext;
        private ReducedTablesDbContext _reducedTablesDbContext;
        private JsonColumnDbContext _jsonColumnDbContext;

        [GlobalSetup]
        public async void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<FullTablesDbContext>();
            services.AddDbContext<ReducedTablesDbContext>();
            services.AddDbContext<JsonColumnDbContext>();
            services.AddSingleton(Helper.GetFhirRelationalOptions<FullTablesDbContext>());
            services.AddSingleton(Helper.GetFhirRelationalOptions<ReducedTablesDbContext>());

            var provider = services.BuildServiceProvider();
            _fullTablesDbContext = provider.GetRequiredService<FullTablesDbContext>();
            _reducedTablesDbContext = provider.GetRequiredService<ReducedTablesDbContext>();
            _jsonColumnDbContext = provider.GetRequiredService<JsonColumnDbContext>();
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
    }
}
