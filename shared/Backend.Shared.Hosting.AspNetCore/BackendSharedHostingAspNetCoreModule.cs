using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace Backend.Shared.Hosting.AspNetCore;

[DependsOn(
    typeof(BackendSharedHostingModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule)
)]
public class BackendSharedHostingAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureSameSiteCookiePolicy(context);
    }
    
    private void ConfigureSameSiteCookiePolicy(ServiceConfigurationContext context)
    {
        context.Services.AddSameSiteCookiePolicy();
    }
}