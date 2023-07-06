using Backend.AdministrationService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Backend.AdministrationService;

[DependsOn(
    typeof(AdministrationServiceEntityFrameworkCoreTestModule)
    )]
public class AdministrationServiceDomainTestModule : AbpModule
{

}
