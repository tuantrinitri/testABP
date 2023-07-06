using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Backend.MedicineService;

[DependsOn(
    typeof(MedicineServiceDomainModule),
    typeof(MedicineServiceApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
)]
public class MedicineServiceApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<MedicineServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MedicineServiceApplicationModule>();
        });
    }
}
