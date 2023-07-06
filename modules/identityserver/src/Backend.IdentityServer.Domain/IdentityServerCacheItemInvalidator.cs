using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.IdentityServer.ApiResources;
using Backend.IdentityServer.ApiScopes;
using Backend.IdentityServer.Clients;
using Backend.IdentityServer.IdentityResources;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.IdentityServer;

namespace Backend.IdentityServer;

public class IdentityServerCacheItemInvalidator :
    ILocalEventHandler<EntityChangedEventData<Client>>,
    ILocalEventHandler<EntityChangedEventData<ClientCorsOrigin>>,
    ILocalEventHandler<EntityChangedEventData<IdentityResource>>,
    ILocalEventHandler<EntityChangedEventData<ApiResource>>,
    ILocalEventHandler<EntityChangedEventData<ApiScope>>,
    ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public IdentityServerCacheItemInvalidator(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<Client> eventData)
    {
        var clientCache = ServiceProvider.GetRequiredService<IDistributedCache<IdentityServer4.Models.Client>>();
        await clientCache.RemoveAsync(eventData.Entity.ClientId, considerUow: true);

        var corsCache = ServiceProvider.GetRequiredService<IDistributedCache<AllowedCorsOriginsCacheItem>>();
        await corsCache.RemoveAsync(AllowedCorsOriginsCacheItem.AllOrigins);
    }

    public async Task HandleEventAsync(EntityChangedEventData<ClientCorsOrigin> eventData)
    {
        var corsCache = ServiceProvider.GetRequiredService<IDistributedCache<AllowedCorsOriginsCacheItem>>();
        await corsCache.RemoveAsync(AllowedCorsOriginsCacheItem.AllOrigins);
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<IdentityResource> eventData)
    {
        var cache = ServiceProvider.GetRequiredService<IDistributedCache<IdentityServer4.Models.IdentityResource>>();
        await cache.RemoveAsync(eventData.Entity.Name);

        var resourcesCache = ServiceProvider.GetRequiredService<IDistributedCache<IdentityServer4.Models.Resources>>();
        await resourcesCache.RemoveAsync(ResourceStore.AllResourcesKey);
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<ApiResource> eventData)
    {
        var cache = ServiceProvider.GetRequiredService<IDistributedCache<IdentityServer4.Models.ApiResource>>();
        await cache.RemoveAsync(ResourceStore.ApiResourceNameCacheKeyPrefix + eventData.Entity.Name);
        await cache.RemoveManyAsync(eventData.Entity.Scopes.Select(x => ResourceStore.ApiResourceScopeNameCacheKeyPrefix + x.Scope));

        var resourcesCache = ServiceProvider.GetRequiredService<IDistributedCache<IdentityServer4.Models.Resources>>();
        await resourcesCache.RemoveAsync(ResourceStore.AllResourcesKey);
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<ApiScope> eventData)
    {
        var cache = ServiceProvider.GetRequiredService<IDistributedCache<IdentityServer4.Models.ApiScope>>();
        await cache.RemoveAsync(eventData.Entity.Name);

        var resourcesCache = ServiceProvider.GetRequiredService<IDistributedCache<IdentityServer4.Models.Resources>>();
        await resourcesCache.RemoveAsync(ResourceStore.AllResourcesKey);
    }
}
