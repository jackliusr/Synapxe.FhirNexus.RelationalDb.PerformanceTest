using Hl7.Fhir.Model;
using Hl7.Fhir.Specification.Summary;
using Ihis.FhirEngine.Core.Utility;
using Ihis.FhirEngine.Data;
using Ihis.FhirEngine.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;

namespace Synapxe.FhirNexus.RelationalDb.PerformanceTest
{
    internal class Helper
    {
        public static OrganizationEntity CreateOrganizationEntity(int hciCode)
        {
            return new OrganizationEntity
            {
                Id = Guid.NewGuid().ToString(),
                VersionId = 1,
                Identifier =
                [
                    new ()
                    {
                        System = "http://ihis.sg/identifier/hci-code",
                        Value = $"hci-{hciCode}",
                    },
                ],
                Name = $"Test Organization {DateTime.Now:HHmmssfffffff}",
                Active = true,
                Type = [
                    new ()
                    {
                        Coding =
                        [
                            new ()
                            {
                                System = "http://ihis.sg/ValueSet/hsg-organization-type",
                                Code = "gp",
                            },
                        ],
                    },
                ],
                Telecom = [
                    new ()
                    {
                       System = ContactPoint.ContactPointSystem.Phone.ToString(),
                       Value = $"8{Random.Shared.Next(1000000,9999999)}",
                    },
                ],
                Address = [
                    new () 
                    {
                        Line = new StringsEntity("123 Test Street"),
                        City = "Test City",
                        State = "Test State",
                        PostalCode = "12345",
                        Country = "Test Country",
                        Type = Address.AddressType.Both.ToString(),
                        Text = "123 Test Street, Test City, Test State, 12345, Test Country",
                    },
                ],
                Extension =[
                    new ()
                    {
                        Url = new Uri("http://ihis.sg/extension/enrollment-capacity"),
                        Value = CreateIntegerDataEntity(Random.Shared.Next(10, 800)),
                    },
                    new()
                    {
                        Url = new Uri("http://ihis.sg/extension/organization-status"),
                        Value = CreateCodeableConceptCodingDataEntity("http://ihis.sg/ValueSet/hsg-organization-status", "provisional" ),
                    },
                    new ()
                    {
                        Url = new Uri("http://ihis.sg/extension/capacity-status"),
                        Value = CreateStringDataEntity("available"),
                    },
                    new ()
                    {
                        Url = new Uri("http://ihis.sg/extension/org-opening-hours"),
                        Value = CreateStringDataEntity("9:00am to 6:00pm"),
                    },
                ],
            };
        }

        public static DataEntity CreateIntegerDataEntity(int value)
        {
            var dataEntity = new DataEntity();
            dataEntity.Integer = value;
            dataEntity.TypeName = DataTypeName.Integer;
            return dataEntity;
        }

        public static DataEntity CreateStringDataEntity(string value)
        {
            var dataEntity = new DataEntity();
            dataEntity.String = value;
            dataEntity.TypeName = DataTypeName.String;
            return dataEntity;
        }

        public 
            static DataEntity CreateCodeableConceptCodingDataEntity(string system, string code)
        {
            var dataEntity = new DataEntity();
            dataEntity.CodeableConceptCoding1 = new CodingEntity
            {
                System = system,
                Code = code,
            };
            dataEntity.TypeName = DataTypeName.CodeableConcept;
            return dataEntity;
        }

        public static FhirRelationalOptions<T> GetFhirRelationalOptions<T>() where T : DbContext
        {
            return new FhirRelationalOptions<T>()
            {
                //DefaultCollation = "Latin1_General_100_CI_AI_SC",
                //CaseInsensitiveCollation = "Latin1_General_100_CI_AI_SC",
                //CaseSensitiveCollation = "Latin1_General_100_CS_AS",
                CreateCompositeKeys = false,
                IsVersioned = true,
                JsonValueMethod = ReflectionUtil.GetMethodInfo(() => SqlServerJsonFunctions.JsonValue(string.Empty, string.Empty))
            };
        }

        public static Organization CreateOrganization()
        {
            var organization = new Organization
            {
                Identifier =
                [
                    new ()
                    {
                        System = "http://ihis.sg/identifier/hci-code",
                        Value = $"hci-{Random.Shared.Next(10000,99999)}",
                    },
                ],
                Name = $"Test Organization {DateTime.Now:HHmmssfffffff}",
                Active = true,
                Type = [
                    new () 
                    {
                        Coding = [
                            new ()
                            {
                                System = "http://ihis.sg/ValueSet/hsg-organization-type",
                                Code = "gp",
                            },
                        ],
                    }
                ],
                Telecom = [
                    new() {
                       System = ContactPoint.ContactPointSystem.Phone,
                       Value = $"8{Random.Shared.Next(1000000,9999999)}",
                    },
                ],
                Address = [
                    new() 
                    {
                        Line = ["123 Test Street"],
                        City = "Test City",
                        State = "Test State",
                        PostalCode = "12345",
                        Country = "Test Country",
                        Type = Address.AddressType.Both,
                        Text = "123 Test Street, Test City, Test State, 12345, Test Country",
                    },
                ],
                Extension = [
                    new() 
                    {
                        Url = "http://ihis.sg/extension/enrollment-capacity",
                        Value = new Integer(500),
                    },
                    new() {
                        Url = "http://ihis.sg/extension/organization-status",
                        Value= new CodeableConcept{
                            Coding = [
                                new() {
                                    System = "http://ihis.sg/ValueSet/hsg-organization-status",
                                    Code = "provisional",
                                }
                            ],
                        },
                    },
                    new() {
                        Url = "http://ihis.sg/extension/capacity-status",
                        Value = new FhirString("available"), // Is FhirString correct to represent a code?
                    },
                    new() 
                    {
                        Url = "http://ihis.sg/extension/org-opening-hours",
                        Value = new FhirString("9:00am to 6:00pm"),
                    },
                ],
            };

            return organization;
        }

        public static async Task PrepareDatabase()
        {
            var services = new ServiceCollection();
            services.AddDbContext<FullTablesDbContext>();
            services.AddDbContext<ReducedTablesDbContext>();
            services.AddDbContext<JsonColumnDbContext>();
            services.AddSingleton(Helper.GetFhirRelationalOptions<FullTablesDbContext>());
            services.AddSingleton(Helper.GetFhirRelationalOptions<ReducedTablesDbContext>());


            services.AddDbContext<FullTablesDbContextPg>();
            services.AddDbContext<ReducedTablesDbContextPg>();
            services.AddDbContext<JsonColumnDbContextPg>();
            services.AddSingleton(Helper.GetFhirRelationalOptions<FullTablesDbContextPg>());
            services.AddSingleton(Helper.GetFhirRelationalOptions<ReducedTablesDbContextPg>());

            var provider = services.BuildServiceProvider();
            var _fullTablesDbContext = provider.GetRequiredService<FullTablesDbContext>();
            var _reducedTablesDbContext = provider.GetRequiredService<ReducedTablesDbContext>();
            var _jsonColumnDbContext = provider.GetRequiredService<JsonColumnDbContext>();

            var _fullTablesDbContextPg = provider.GetRequiredService<FullTablesDbContextPg>();
            var _reducedTablesDbContextPg = provider.GetRequiredService<ReducedTablesDbContextPg>();
            var _jsonColumnDbContextPg = provider.GetRequiredService<JsonColumnDbContextPg>();
            
            int hciCode = 10000;
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    var organization = Helper.CreateOrganizationEntity(hciCode++);
                    await _fullTablesDbContext.AddAsync(organization);
                    await _reducedTablesDbContext.AddAsync(organization);
                    await _jsonColumnDbContext.AddAsync(organization);

                    await _fullTablesDbContextPg.AddAsync(organization);
                    await _reducedTablesDbContextPg.AddAsync(organization);
                    await _jsonColumnDbContextPg.AddAsync(organization);
                }
                await _fullTablesDbContext.SaveChangesAsync();
                await _reducedTablesDbContext.SaveChangesAsync();
                await _jsonColumnDbContext.SaveChangesAsync();

                await _fullTablesDbContextPg.SaveChangesAsync();
                await _reducedTablesDbContextPg.SaveChangesAsync();
                await _jsonColumnDbContextPg.SaveChangesAsync();
            }
        }
    }
}
