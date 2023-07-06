using Backend.MedicineService.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Backend.MedicineService;

[DependsOn(
    typeof(AbpValidationModule)
    )]
public class MedicineServiceDomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        MedicineServiceGlobalFeatureConfigurator.Configure();
        MedicineServiceModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<MedicineServiceDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<MedicineServiceResource>("vi")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/MedicineService");

            options.DefaultResourceType = typeof(MedicineServiceResource);
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("MedicineService", typeof(MedicineServiceResource));
        });
    }
}
