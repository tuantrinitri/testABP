using Backend.IdentityServer.Clients;
using Backend.IdentityServer.Devices;
using Backend.IdentityServer.Grants;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.IdentityServer;

public static class IdentityServerBuilderExtensions
{
    public static IIdentityServerBuilder AddAbpStores(this IIdentityServerBuilder builder)
    {
        builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

        return builder
            .AddClientStore<ClientStore>()
            .AddResourceStore<ResourceStore>()
            .AddCorsPolicyService<AbpCorsPolicyService>();
    }
}
