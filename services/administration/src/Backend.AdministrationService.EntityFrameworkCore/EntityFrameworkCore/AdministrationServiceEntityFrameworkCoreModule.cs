using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Backend.AdministrationService.EntityFrameworkCore;

[DependsOn(
    typeof(AdministrationServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule)
)]
public class AdministrationServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AdministrationServiceDbContext>(options =>
        {
            options.ReplaceDbContext<IPermissionManagementDbContext>();
            options.ReplaceDbContext<ISettingManagementDbContext>();
            options.ReplaceDbContext<IAuditLoggingDbContext>();
            options.ReplaceDbContext<IBlobStoringDbContext>();
            
            // options.AddDefaultRepositories<IPermissionManagementDbContext>();
            //
            // options.AddRepository<PermissionGroupDefinitionRecord, EfCorePermissionGroupDefinitionRecordRepository>();
            // options.AddRepository<PermissionDefinitionRecord, EfCorePermissionDefinitionRecordRepository>();
            // options.AddRepository<PermissionGrant, EfCorePermissionGrantRepository>();

            options.AddDefaultRepositories(includeAllEntities: true);
        });
        
        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<AdministrationServiceDbContext>(c =>
            {
                c.UseSqlServer(b =>
                {
                    b.MigrationsHistoryTable("__AdministrationService_Migrations");
                });
            });
        });
    }
}
