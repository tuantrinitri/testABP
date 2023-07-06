using Backend.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Backend.IdentityService;

[DependsOn(
    typeof(IdentityServiceDomainModule),
    typeof(IdentityServiceApplicationContractsModule),
    typeof(IdentityApplicationModule)
)]
public class IdentityServiceApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IdentityServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<IdentityServiceApplicationModule>(validate: true);
        });
    }
}
