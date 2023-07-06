using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Backend.MedicineService.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class MedicineServiceDbContextFactory : IDesignTimeDbContextFactory<MedicineServiceDbContext>
{
    public MedicineServiceDbContext CreateDbContext(string[] args)
    {
        MedicineServiceEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<MedicineServiceDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new MedicineServiceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Backend.MedicineService.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
