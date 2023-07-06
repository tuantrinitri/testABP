using Backend.Identity.Identity;
using Backend.Identity.IdentityUsers;
using Backend.Identity.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.VirtualFileSystem;

namespace Backend.Identity;

[DependsOn(
    typeof(IdentityEntityFrameworkCoreTestModule),
    typeof(IdentityTestBaseModule),
    typeof(PermissionManagementDomainIdentityModule)
    )]
public class IdentityDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<IdentityUser>();
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityDomainTestModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityResource>()
                .AddVirtualJson("/LocalizationExtensions");
        });

        Configure<PermissionManagementOptions>(options =>
        {
            options.IsDynamicPermissionStoreEnabled = false;
            options.SaveStaticPermissionsToDatabase = false;
        });

        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });

        Configure<AbpSettingOptions>(options =>
        {
            options.ValueProviders.Add<TestSettingValueProvider>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(() => scope.ServiceProvider
                .GetRequiredService<TestPermissionDataBuilder>()
                .Build());
        }
    }
}
