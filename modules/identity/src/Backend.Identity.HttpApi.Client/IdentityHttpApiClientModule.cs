using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Backend.Identity.HttpApi.Client;

[DependsOn(
    typeof(IdentityApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class IdentityHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(IdentityApplicationContractsModule).Assembly,
            IdentityRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityHttpApiClientModule>();
        });
    }
}