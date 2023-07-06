using Backend.Identity.Grants;
using Backend.Identity.IdentityResources;
using Backend.IdentityServer.ApiResources;
using Backend.IdentityServer.ApiScopes;
using Backend.IdentityServer.Clients;
using Backend.IdentityServer.Devices;
using Backend.IdentityServer.Grants;
using Backend.IdentityServer.IdentityResources;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Backend.IdentityServer.EntityFrameworkCore;

[DependsOn(
    typeof(IdentityServerDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class IdentityServerEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<IIdentityServerBuilder>(
            builder =>
            {
                builder.AddAbpStores();
            }
        );
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityServerDbContext>(options =>
        {
            options.AddDefaultRepositories<IIdentityServerDbContext>();

            options.AddRepository<Client, ClientRepository>();
            options.AddRepository<ApiResource, ApiResourceRepository>();
            options.AddRepository<ApiScope, ApiScopeRepository>();
            options.AddRepository<IdentityResource, IdentityResourceRepository>();
            options.AddRepository<PersistedGrant, PersistentGrantRepository>();
            options.AddRepository<DeviceFlowCodes, DeviceFlowCodesRepository>();
        });
    }
}
