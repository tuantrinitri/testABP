using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Backend.IdentityServer;

[DependsOn(
    typeof(AbpValidationModule)
    )]
public class IdentityServerDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityServerDomainSharedModule>();
        });

        // Configure<AbpLocalizationOptions>(options =>
        // {
        //     options.Resources.Add<AbpIdentityServerResource>("en")
        //         .AddBaseTypes(
        //             typeof(AbpValidationResource)
        //         ).AddVirtualJson("/Volo/Abp/IdentityServer/Localization/Resources");
        // });
        //
        // Configure<AbpExceptionLocalizationOptions>(options =>
        // {
        //     options.MapCodeNamespace("Volo.IdentityServer", typeof(AbpIdentityServerResource));
        // });
    }
}
