using Backend.Identity;
using Backend.IdentityServer;
using Backend.IdentityService.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Backend.IdentityService
{
    [DependsOn(
        typeof(IdentityDomainSharedModule),
        typeof(IdentityServerDomainSharedModule)
        )]
    public class IdentityServiceDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            IdentityServiceModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<IdentityServiceDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<IdentityServiceResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/IdentityService");

                options.DefaultResourceType = typeof(IdentityServiceResource);
            });
        }
    }
}
