using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Users.EntityFrameworkCore;

namespace Backend.Identity.EntityFrameworkCore;

[DependsOn(
    typeof(IdentityDomainModule),
    typeof(AbpUsersEntityFrameworkCoreModule))]
public class IdentityEntityFrameworkCoreModule: AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityDbContext>(options =>
        {
            // options.AddDefaultRepositories(includeAllEntities: true);
        });
    }
}