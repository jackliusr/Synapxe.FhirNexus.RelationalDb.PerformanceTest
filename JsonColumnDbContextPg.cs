using Ihis.FhirEngine.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    internal class JsonColumnDbContextPg : DbContext
    {
        public JsonColumnDbContextPg(DbContextOptions<JsonColumnDbContextPg> options)
            : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<OrganizationEntity> Organization => Set<OrganizationEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=performancetest_jsoncolumn;Username=postgres;Password=Orange1SG", o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var options = Helper.GetFhirRelationalOptions<JsonColumnDbContextPg>();
            modelBuilder.UseCollation(options.DefaultCollation);
            modelBuilder.HasDbFunction(options.JsonValueMethod);

            modelBuilder.Entity<OrganizationEntity>()
                .OwnsMany(o => o.Identifier, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.Ignore(x => x.Assigner);
                    ownedNavigationBuilder.OwnsOne(x => x.Period, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.OwnsOne(m => m.Start);
                        ownedNavigationBuilder.OwnsOne(m => m.End);
                    });
                    ownedNavigationBuilder.OwnsOne(x => x.Type, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.OwnsMany(t => t.Coding);
                    });
                })
                .OwnsMany(o => o.Type, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.OwnsMany(a => a.Coding);
                })
                .OwnsMany(o => o.Telecom, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.OwnsOne(x => x.Period, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.OwnsOne(m => m.Start);
                        ownedNavigationBuilder.OwnsOne(m => m.End);
                    });
                })
                .OwnsMany(o => o.Address, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.OwnsOne(a => a.Line);
                    ownedNavigationBuilder.OwnsOne(a => a.Period, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.OwnsOne(m => m.Start);
                        ownedNavigationBuilder.OwnsOne(m => m.End);
                    });
                })
                .OwnsMany(o => o.Contact, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.OwnsOne(c => c.Name, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.OwnsOne(n => n.Given);
                        ownedNavigationBuilder.OwnsOne(n => n.Prefix);
                        ownedNavigationBuilder.OwnsOne(n => n.Suffix);
                        ownedNavigationBuilder.OwnsOne(n => n.Period, ownedNavigationBuilder =>
                        {
                            ownedNavigationBuilder.OwnsOne(m => m.Start);
                            ownedNavigationBuilder.OwnsOne(m => m.End);
                        });
                    });
                    ownedNavigationBuilder.OwnsOne(c => c.Address, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.OwnsOne(t => t.Line);
                        ownedNavigationBuilder.OwnsOne(t => t.Period, ownedNavigationBuilder =>
                        {
                            ownedNavigationBuilder.OwnsOne(m => m.Start);
                            ownedNavigationBuilder.OwnsOne(m => m.End);
                        });
                    });
                    ownedNavigationBuilder.OwnsOne(c => c.Purpose, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.OwnsMany(x=>x.Coding);
                    });
                    ownedNavigationBuilder.OwnsMany(c => c.Telecom, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.OwnsOne(x=>x.Period, ownedNavigationBuilder =>
                        {
                            ownedNavigationBuilder.OwnsOne(m => m.Start);
                            ownedNavigationBuilder.OwnsOne(m => m.End);
                        });
                    });
                    ownedNavigationBuilder.Ignore(c => c.Extension);
                    ownedNavigationBuilder.Ignore(c => c.ModifierExtension);
                })
                .OwnsMany(o => o.Endpoint, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.OwnsOne(x => x.Identifier, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.Ignore(i => i.Period);
                    });
                })
                .OwnsMany(o => o.Contained, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.Ignore(x => x.Id); // https://github.com/dotnet/efcore/issues/32360
                })
                .OwnsMany(o => o.Extension, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.OwnsOne(a => a.Value, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.Ignore(d => d.Reference);
                        ownedNavigationBuilder.OwnsOne(d => d.CodeableConceptCoding1);
                        ownedNavigationBuilder.OwnsOne(d => d.CodeableConceptCoding2);
                        ownedNavigationBuilder.OwnsOne(d => d.Coding);
                        ownedNavigationBuilder.OwnsOne(d => d.SampledData, ownedNavigationBuilder =>
                        {
                            ownedNavigationBuilder.OwnsOne(d => d.Origin);
                        });
                        ownedNavigationBuilder.OwnsOne(d => d.Quantity);
                        ownedNavigationBuilder.OwnsOne(d => d.DateTime);
                    });
                })
                .OwnsMany(o => o.ModifierExtension, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                    ownedNavigationBuilder.OwnsOne(a => a.Value, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.Ignore(d => d.Reference);
                        ownedNavigationBuilder.OwnsOne(d => d.CodeableConceptCoding1);
                        ownedNavigationBuilder.OwnsOne(d => d.CodeableConceptCoding2);
                        ownedNavigationBuilder.OwnsOne(d => d.Coding);
                        ownedNavigationBuilder.OwnsOne(d => d.SampledData, ownedNavigationBuilder =>
                        {
                            ownedNavigationBuilder.OwnsOne(d => d.Origin);
                        });
                        ownedNavigationBuilder.OwnsOne(d => d.Quantity);
                        ownedNavigationBuilder.OwnsOne(d => d.DateTime);
                    });
                });

            modelBuilder.Entity<OrganizationEntity>().OwnsOne(o => o.Alias).ToJson();
            //modelBuilder.Entity<OrganizationEntity>().OwnsOne(o => o.PartOf, ownedNavigationBuilder =>
            //{
            //    ownedNavigationBuilder.OwnsOne(p => p.Identifier, ownedNavigationBuilder =>
            //    {
            //        ownedNavigationBuilder.Ignore(i => i.Period);
            //    });
            //});
            modelBuilder.Entity<OrganizationEntity>().Ignore(o => o.PartOf);

            modelBuilder.Entity<OrganizationEntity>().OwnsOne(o => o.Meta, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsMany(x => x.Profile);
                ownedNavigationBuilder.OwnsMany(x => x.Security);
                ownedNavigationBuilder.OwnsMany(x => x.Tag);
            });
            modelBuilder.Entity<OrganizationEntity>().OwnsOne(o => o.Text).ToJson();
        }
    }
}
