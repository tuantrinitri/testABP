using Backend.MedicineService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Backend.MedicineService;

[DependsOn(
    typeof(MedicineServiceEntityFrameworkCoreTestModule)
    )]
public class MedicineServiceDomainTestModule : AbpModule
{

}
