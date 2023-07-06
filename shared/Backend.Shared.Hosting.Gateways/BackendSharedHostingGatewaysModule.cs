using Backend.Shared.Hosting.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Backend.Shared.Hosting.Gateways;

[DependsOn(
    typeof(BackendSharedHostingAspNetCoreModule)
)]
public class BackendSharedHostingGatewaysModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
            
        context.Services.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }
}