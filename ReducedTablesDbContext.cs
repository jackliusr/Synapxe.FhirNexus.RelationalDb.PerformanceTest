﻿using Ihis.FhirEngine.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    internal class ReducedTablesDbContext : DbContext
    {
        public ReducedTablesDbContext(DbContextOptions<ReducedTablesDbContext> options)
            : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<OrganizationEntity> Organization => Set<OrganizationEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PerformanceTest_ReducedTables;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseFhirConventions(this, true);

            modelBuilder.Entity<OrganizationEntity>()
                .Ignore(x => x.Alias)
                .Ignore(x => x.PartOf)
                .Ignore(x => x.Endpoint)
                .Ignore(x => x.Text)
                .Ignore(x => x.Language)
                .Ignore(x => x.Contained)
                .Ignore(x => x.ModifierExtension)
                .Ignore(x => x.Contact)
                .OwnsOne(x => x.Meta, meta =>
                    meta.Ignore(x => x.Profile)
                        .Ignore(x => x.Security)
                        .Ignore(x => x.Tag))
                .OwnsMany(x => x.Identifier, identifier =>
                    identifier.Ignore(x => x.Type)
                              .Ignore(x => x.Assigner))
                .OwnsMany(x => x.Telecom, telcom =>
                    telcom.Ignore(x => x.Use)
                          .Ignore(x => x.Rank)
                          .Ignore(x => x.Period))
                .OwnsMany(x => x.Address, address =>
                    address.Ignore(x => x.Period)
                           .Ignore(x => x.City)
                           .Ignore(x => x.District)
                           .Ignore(x => x.State)
                           .Ignore(x => x.Country));
        }
    }
}
