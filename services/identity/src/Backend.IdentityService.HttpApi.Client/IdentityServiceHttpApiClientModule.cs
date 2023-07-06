using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Backend.IdentityService;

[DependsOn(
    typeof(IdentityServiceApplicationContractsModule)
)]
public class IdentityServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // context.Services.AddHttpClientProxies(
        //     typeof(IdentityServiceApplicationContractsModule).Assembly,
        //     IdentityServiceRemoteServiceConsts.RemoteServiceName
        // );
    }
}