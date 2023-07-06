using Backend.Identity;
using Backend.IdentityServer;
using Volo.Abp.Modularity;

namespace Backend.IdentityService;

[DependsOn(
    typeof(IdentityServiceDomainSharedModule),
    typeof(IdentityDomainModule),
    typeof(IdentityServerDomainModule)
)]
public class IdentityServiceDomainModule : AbpModule
{
}