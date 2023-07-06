using System.Threading.Tasks;
using Backend.Identity.Etos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.PermissionManagement;

namespace Backend.Identity;

public class RoleUpdateEventHandler :
    IDistributedEventHandler<IdentityRoleNameChangedEto>,
    ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }
    protected IPermissionGrantRepository PermissionGrantRepository { get; }

    public RoleUpdateEventHandler(
        IPermissionManager permissionManager,
        IPermissionGrantRepository permissionGrantRepository)
    {
        PermissionManager = permissionManager;
        PermissionGrantRepository = permissionGrantRepository;
    }

    public async Task HandleEventAsync(IdentityRoleNameChangedEto eventData)
    {
        var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync(RolePermissionValueProvider.ProviderName, eventData.OldName);
        foreach (var permissionGrant in permissionGrantsInRole)
        {
            await PermissionManager.UpdateProviderKeyAsync(permissionGrant, eventData.Name);
        }
    }
}
