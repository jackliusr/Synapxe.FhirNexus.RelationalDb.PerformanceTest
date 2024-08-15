using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    public class SearchByIdentifierBenchmark
    {
        private string hcicode;
        private static Uri uri = new Uri("http://ihis.sg/extension/enrollment-capacity");
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
            _fullTablesDbContext.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Url == uri && y.Value.Integer > 450)).ToList();
        }

        [Benchmark]
        public void ReducedTablesDbContext()
        {
            _reducedTablesDbContext.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Url == uri && y.Value.Integer > 450)).ToList();
        }

        [Benchmark]
        public void JsonColumnDbContext()
        {
            _jsonColumnDbContext.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Url == uri && y.Value.Integer > 450)).ToList();
        }
    }
}
