using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Backend.MedicineService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(MedicineServiceDomainSharedModule)
)]
public class MedicineServiceDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
       
    }
}
