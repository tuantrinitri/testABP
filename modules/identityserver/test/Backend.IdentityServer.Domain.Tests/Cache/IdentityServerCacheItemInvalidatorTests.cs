﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.IdentityServer.ApiResources;
using Backend.IdentityServer.ApiScopes;
using Backend.IdentityServer.Clients;
using Backend.IdentityServer.EntityFrameworkCore;
using Backend.IdentityServer.IdentityResources;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Shouldly;
using Volo.Abp.Caching;
using Xunit;
using ApiResource = Backend.IdentityServer.ApiResources.ApiResource;
using ApiScope = Backend.IdentityServer.ApiScopes.ApiScope;
using Client = Backend.IdentityServer.Clients.Client;
using IdentityResource = Backend.IdentityServer.IdentityResources.IdentityResource;

namespace Backend.IdentityServer.Cache;

public class IdentityServerCacheItemInvalidatorTests : IdentityServerTestBase
{
    private readonly IClientStore _clientStore;
    private readonly IResourceStore _resourceStore;

    private readonly IClientRepository _clientRepository;
    private readonly IIdentityResourceRepository _identityResourceRepository;
    private readonly IApiResourceRepository _apiResourceRepository;
    private readonly IApiScopeRepository _apiScopeRepository;

    private readonly IDistributedCache<Client> _clientCache;
    private readonly IDistributedCache<IdentityResource> _identityResourceCache;
    private readonly IDistributedCache<IEnumerable<ApiResource>> _apiResourcesCache;
    private readonly IDistributedCache<ApiResource> _apiResourceCache;
    private readonly IDistributedCache<ApiScope> _apiScopeCache;
    private readonly IDistributedCache<Resources> _resourceCache;

    private readonly IdentityServerTestData _testData;

    public IdentityServerCacheItemInvalidatorTests()
    {
        _clientStore = GetRequiredService<IClientStore>();
        _resourceStore = GetRequiredService<IResourceStore>();

        _clientRepository = GetRequiredService<IClientRepository>();
        _identityResourceRepository = GetRequiredService<IIdentityResourceRepository>();
        _apiResourceRepository = GetRequiredService<IApiResourceRepository>();
        _apiScopeRepository = GetRequiredService<IApiScopeRepository>();

        _clientCache = GetRequiredService<IDistributedCache<Client>>();
        _identityResourceCache = GetRequiredService<IDistributedCache<IdentityResource>>();
        _apiResourcesCache = GetRequiredService<IDistributedCache<IEnumerable<ApiResource>>>();
        _apiResourceCache = GetRequiredService<IDistributedCache<ApiResource>>();
        _apiScopeCache = GetRequiredService<IDistributedCache<ApiScope>>();
        _resourceCache = GetRequiredService<IDistributedCache<Resources>>();

        _testData = GetRequiredService<IdentityServerTestData>();
    }

    [Fact]
    public async Task Models_Should_Cached_And_Invalidator_When_Its_Changed()
    {
        //client
        var clientId = "ClientId1";
        (await _clientCache.GetAsync(clientId)).ShouldBeNull();

        var client = await _clientStore.FindClientByIdAsync(clientId);
        client.ShouldNotBeNull();

        var clientCacheItem = await _clientCache.GetAsync(clientId);
        clientCacheItem.ShouldNotBeNull();

        await _clientRepository.DeleteAsync(_testData.Client1Id);
        (await _clientCache.GetAsync(clientId)).ShouldBeNull();


        //Api Resource
        var newApiResource1 = "NewApiResource1";
        var newApiResource2 = "NewApiResource2";
        var testApiResourceName1 = "Test-ApiResource-Name-1";
        var testApiResourceApiScopeName1 = "Test-ApiResource-ApiScope-Name-1";
        var newApiResources = new[] { newApiResource1, newApiResource2 };

        //FindApiResourcesByNameAsync
        (await _apiResourceCache.GetAsync(newApiResource1)).ShouldBeNull();
        (await _apiResourceCache.GetAsync(newApiResource2)).ShouldBeNull();
        await _resourceStore.FindApiResourcesByNameAsync(newApiResources);
        (await _apiResourceCache.GetAsync(ResourceStore.ApiResourceNameCacheKeyPrefix + newApiResource1)).ShouldNotBeNull();
        (await _apiResourceCache.GetAsync(ResourceStore.ApiResourceNameCacheKeyPrefix + newApiResource2)).ShouldNotBeNull();

        var apiResource1 = await _apiResourceRepository.FindByNameAsync(newApiResource1);
        await _apiResourceRepository.DeleteAsync(apiResource1);
        (await _apiResourceCache.GetAsync(newApiResource1)).ShouldBeNull();

        var apiResource2 = await _apiResourceRepository.FindByNameAsync(newApiResource2);
        await _apiResourceRepository.DeleteAsync(apiResource2);
        (await _apiResourceCache.GetAsync(newApiResource2)).ShouldBeNull();

        //FindApiResourcesByScopeNameAsync
        (await _apiResourcesCache.GetAsync(ResourceStore.ApiResourceScopeNameCacheKeyPrefix + testApiResourceApiScopeName1)).ShouldBeNull();
        await _resourceStore.FindApiResourcesByScopeNameAsync(new[] { testApiResourceApiScopeName1 });
        (await _apiResourcesCache.GetAsync(ResourceStore.ApiResourceScopeNameCacheKeyPrefix + testApiResourceApiScopeName1)).ShouldNotBeNull();

        var testApiResource1 = await _apiResourceRepository.FindByNameAsync(testApiResourceName1);
        await _apiResourceRepository.DeleteAsync(testApiResource1);
        (await _apiResourceCache.GetAsync(ResourceStore.ApiResourceScopeNameCacheKeyPrefix + testApiResourceApiScopeName1)).ShouldBeNull();


        //Identity Resource
        var testIdentityResourceName = "Test-Identity-Resource-Name-1";
        var testIdentityResourceNames = new[] { testIdentityResourceName };
        (await _identityResourceCache.GetAsync(testIdentityResourceName)).ShouldBeNull();
        await _resourceStore.FindIdentityResourcesByScopeNameAsync(testIdentityResourceNames);
        (await _identityResourceCache.GetAsync(testIdentityResourceName)).ShouldNotBeNull();

        var testIdentityResource = await _identityResourceRepository.FindByNameAsync(testIdentityResourceName);
        await _identityResourceRepository.DeleteAsync(testIdentityResource);
        (await _identityResourceCache.GetAsync(testIdentityResourceName)).ShouldBeNull();


        //Api Scope
        var testApiScopeName = "Test-ApiScope-Name-1";
        var testApiScopeNames = new[] { testApiScopeName };
        (await _apiScopeCache.GetAsync(testApiScopeName)).ShouldBeNull();
        await _resourceStore.FindApiScopesByNameAsync(testApiScopeNames);
        (await _apiScopeCache.GetAsync(testApiScopeName)).ShouldNotBeNull();

        var testApiScope = await _apiScopeRepository.FindByNameAsync(testApiScopeName);
        await _apiScopeRepository.DeleteAsync(testApiScope);
        (await _apiScopeCache.GetAsync(testApiScopeName)).ShouldBeNull();


        //Resources
        (await _resourceCache.GetAsync(ResourceStore.AllResourcesKey)).ShouldBeNull();
        await _resourceStore.GetAllResourcesAsync();
        (await _resourceCache.GetAsync(ResourceStore.AllResourcesKey)).ShouldNotBeNull();

        await _identityResourceRepository.DeleteAsync(_testData.IdentityResource1Id);
        (await _resourceCache.GetAsync(ResourceStore.AllResourcesKey)).ShouldBeNull();
    }
}
