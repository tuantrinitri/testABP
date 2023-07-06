using Backend.Identity.EntityFrameworkCore;
using Backend.IdentityServer.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Backend.IdentityService.EntityFrameworkCore
{
    [DependsOn(
        typeof(IdentityServiceDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(IdentityEntityFrameworkCoreModule),
        typeof(IdentityServerEntityFrameworkCoreModule)
    )]
    public class IdentityServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            IdentityServiceEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IdentityServiceDbContext>(options =>
            {
                options.ReplaceDbContext<IIdentityDbContext>();
                options.ReplaceDbContext<IIdentityServerDbContext>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure<IdentityServiceDbContext>(c =>
                {
                    c.UseSqlServer(b => { b.MigrationsHistoryTable("__IdentityService_Migrations"); })
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
            });
            
            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            });
        }
    }
}