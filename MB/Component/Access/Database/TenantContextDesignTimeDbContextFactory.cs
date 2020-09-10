using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MB.Access.Tenant.Database
{
    public class TenantContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TenantContext>
    {
        public TenantContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", true)
                .AddUserSecrets("7bc5f2bc-9213-4a3b-89a6-23cd9f319e74", true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TenantContext>();
            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString($"{nameof(TenantContext)}Dbo"),
                builder => builder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "tenant"));

            return new TenantContext(optionsBuilder.Options);
        }
    }
}
