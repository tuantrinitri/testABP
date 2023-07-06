using Backend.Identity;
using Volo.Abp.Modularity;

namespace Backend.IdentityService;

[DependsOn(
    typeof(IdentityServiceDomainSharedModule),
    typeof(IdentityApplicationContractsModule)
)]
public class IdentityServiceApplicationContractsModule : AbpModule
{
    
}
