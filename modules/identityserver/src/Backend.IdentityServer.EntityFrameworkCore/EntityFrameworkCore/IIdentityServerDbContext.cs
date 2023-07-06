using Backend.IdentityServer.ApiResources;
using Backend.IdentityServer.ApiScopes;
using Backend.IdentityServer.Clients;
using Backend.IdentityServer.Devices;
using Backend.IdentityServer.Grants;
using Backend.IdentityServer.IdentityResources;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Backend.IdentityServer.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(IdentityServerDbProperties.ConnectionStringName)]
public interface IIdentityServerDbContext : IEfCoreDbContext
{
    #region ApiResource

    DbSet<ApiResource> ApiResources { get; }

    DbSet<ApiResourceSecret> ApiResourceSecrets { get; }

    DbSet<ApiResourceClaim> ApiResourceClaims { get; }

    DbSet<ApiResourceScope> ApiResourceScopes { get; }

    DbSet<ApiResourceProperty> ApiResourceProperties { get; }

    #endregion

    #region ApiScope

    DbSet<ApiScope> ApiScopes { get; }

    DbSet<ApiScopeClaim> ApiScopeClaims { get; }

    DbSet<ApiScopeProperty> ApiScopeProperties { get; }

    #endregion

    #region IdentityResource

    DbSet<IdentityResource> IdentityResources { get; }

    DbSet<IdentityResourceClaim> IdentityClaims { get; }

    DbSet<IdentityResourceProperty> IdentityResourceProperties { get; }

    #endregion

    #region Client

    DbSet<Client> Clients { get; }

    DbSet<ClientGrantType> ClientGrantTypes { get; }

    DbSet<ClientRedirectUri> ClientRedirectUris { get; }

    DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; }

    DbSet<ClientScope> ClientScopes { get; }

    DbSet<ClientSecret> ClientSecrets { get; }

    DbSet<ClientClaim> ClientClaims { get; }

    DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; }

    DbSet<ClientCorsOrigin> ClientCorsOrigins { get; }

    DbSet<ClientProperty> ClientProperties { get; }

    #endregion

    DbSet<PersistedGrant> PersistedGrants { get; }

    DbSet<DeviceFlowCodes> DeviceFlowCodes { get; }
}
