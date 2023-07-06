using Volo.Abp.Modularity;

namespace Backend.Identity.Application.Tests;

[DependsOn(
    typeof(IdentityApplicationModule),
    typeof(IdentityDomainTestModule)
    )]
public class AbpIdentityApplicationTestModule : AbpModule
{

}
