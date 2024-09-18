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

        [IterationSetup]
        public void IterationSetup()
        {
            hcicode = $"hci-{Random.Shared.Next(10000, 19999)}";
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

        [Benchmark]
        public void FullTablesDbContextPg()
        {
            _fullTablesDbContextPg.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Url == uri && y.Value.Integer > 450)).ToList();
        }

        [Benchmark]
        public void ReducedTablesDbContextPg()
        {
            _reducedTablesDbContextPg.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Url == uri && y.Value.Integer > 450)).ToList();
        }

        [Benchmark]
        public void JsonColumnDbContextPg()
        {
            _jsonColumnDbContextPg.Organization.Where(x => x.Identifier.FirstOrDefault().Value == hcicode && x.Extension.Any(y => y.Url == uri && y.Value.Integer > 450)).ToList();
        }
    }
}
