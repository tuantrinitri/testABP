using Volo.Abp.Modularity;

namespace Backend.MedicineService;

[DependsOn(
    typeof(MedicineServiceApplicationModule),
    typeof(MedicineServiceDomainTestModule)
    )]
public class MedicineServiceApplicationTestModule : AbpModule
{

}
