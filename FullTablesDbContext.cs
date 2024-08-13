using Ihis.FhirEngine.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    internal class FullTablesDbContext : DbContext
    {
        public FullTablesDbContext(DbContextOptions<FullTablesDbContext> options)
            : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<OrganizationEntity> Organization => Set<OrganizationEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PerformanceTest.FullTables;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseFhirConventions(this, true);
        }
    }
}
