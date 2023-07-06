using Localization.Resources.AbpUi;
using Backend.MedicineService.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Backend.MedicineService;

[DependsOn(
    typeof(MedicineServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule)
    )]
public class MedicineServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(MedicineServiceHttpApiModule).Assembly);
        });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<MedicineServiceResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
