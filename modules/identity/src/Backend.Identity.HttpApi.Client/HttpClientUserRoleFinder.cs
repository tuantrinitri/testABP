using Backend.Identity.Identity;
using Backend.Identity.IdentityUsers;
using Volo.Abp.DependencyInjection;

namespace Backend.Identity.HttpApi.Client;

[Dependency(TryRegister = true)]
public class HttpClientUserRoleFinder : IUserRoleFinder, ITransientDependency
{
    protected IIdentityUserAppService UserAppService { get; }

    public HttpClientUserRoleFinder(IIdentityUserAppService userAppService)
    {
        UserAppService = userAppService;
    }

    public virtual async Task<string[]> GetRolesAsync(Guid userId)
    {
        var output = await UserAppService.GetRolesAsync(userId);
        return output.Items.Select(r => r.Name).ToArray();
    }
}
