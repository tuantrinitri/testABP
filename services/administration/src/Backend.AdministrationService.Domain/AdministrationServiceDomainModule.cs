using Volo.Abp.AuditLogging;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;

namespace Backend.AdministrationService;

[DependsOn(
    typeof(AdministrationServiceDomainSharedModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainIdentityServerModule)
)]
public class AdministrationServiceDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("vi", "vi", "Tiếng Việt", "vi"));
        });
    }
}
