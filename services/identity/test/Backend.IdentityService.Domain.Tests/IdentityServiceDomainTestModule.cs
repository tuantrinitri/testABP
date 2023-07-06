using Backend.IdentityService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Backend.IdentityService;

[DependsOn(
    typeof(IdentityServiceEntityFrameworkCoreTestModule)
    )]
public class IdentityServiceDomainTestModule : AbpModule
{

}
