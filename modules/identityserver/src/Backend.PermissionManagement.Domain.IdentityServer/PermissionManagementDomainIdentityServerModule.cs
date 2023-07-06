using Backend.IdentityServer;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Backend.PermissionManagement.IdentityServer;

[DependsOn(
    typeof(IdentityServerDomainSharedModule),
    typeof(AbpPermissionManagementDomainModule)
)]
public class PermissionManagementDomainIdentityServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<ClientPermissionManagementProvider>();

            options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "IdentityServer.Client.ManagePermissions";
        });
    }
}
