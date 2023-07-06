using Backend.Identity.Identity;
using Backend.Identity.IdentityRoles.Dto;
using Backend.Identity.IdentityUsers.Dto;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Backend.Identity;

public class IdentityApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.Role,
                getApiTypes: new[] { typeof(IdentityRoleDto) },
                createApiTypes: new[] { typeof(IdentityRoleCreateDto) },
                updateApiTypes: new[] { typeof(IdentityRoleUpdateDto) }
            );
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.User,
                getApiTypes: new[] { typeof(IdentityUserDto) },
                createApiTypes: new[] { typeof(IdentityUserCreateDto) },
                updateApiTypes: new[] { typeof(IdentityUserUpdateDto) }
            );
        });
    }
}