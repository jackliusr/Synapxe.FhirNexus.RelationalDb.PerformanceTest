using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    public class ComparePerformance
    {
        private string hcicode;
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

        [IterationSetup]
        public void IterationSetup()
        {
            hcicode = $"hci-{Random.Shared.Next(10000, 99999)}";
        }

        [Benchmark]
        public void FullTablesDbContext()
        {
            _fullTablesDbContext.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Value.Integer > 450)).ToList();
        }

        [Benchmark]
        public void ReducedTablesDbContext()
        {
            _reducedTablesDbContext.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Value.Integer > 450)).ToList();
        }

        [Benchmark]
        public void JsonColumnDbContext()
        {
            _jsonColumnDbContext.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Value.Integer > 450)).ToList();
        }
    }
}
