using Ihis.FhirEngine.Data.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    internal class FullTablesDbContextPg : DbContext
    {
        public FullTablesDbContextPg(DbContextOptions<FullTablesDbContextPg> options)
            : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<OrganizationEntity> Organization => Set<OrganizationEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=performancetest_fulltables;Username=postgres;Password=Orange1SG");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseFhirConventions(this, true);
        }
    }
}
