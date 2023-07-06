using Backend.MedicineService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Backend.MedicineService.Permissions;

public class MedicineServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MedicineServicePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MedicineServicePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MedicineServiceResource>(name);
    }
}
