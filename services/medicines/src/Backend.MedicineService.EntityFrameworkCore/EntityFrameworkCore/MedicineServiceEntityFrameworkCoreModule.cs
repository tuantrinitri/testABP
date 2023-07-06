using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Backend.MedicineService.EntityFrameworkCore;

[DependsOn(
    typeof(MedicineServiceDomainModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule)
    )]
public class MedicineServiceEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        MedicineServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<MedicineServiceDbContext>(options =>
        {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
                /* The main point to change your DBMS.
                 * See also MedicineServiceMigrationsDbContextFactory for EF Core tooling. */
            options.UseSqlServer(b =>
            {
                b.MigrationsHistoryTable("__OrderingService_Migrations");
            });
        });
    }
}
